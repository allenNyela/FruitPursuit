using System.Collections;
using UnityEngine;

public class Anim_Chocolate : MonoBehaviour
{
    [Header("ATTACK TIMING (total = attack + interval)")]
    public float attackDuration   = 1.00f;
    public float intervalDuration = 1.50f;

    [Header("Attack Sub-phases (% of attackDuration)")]
    public float scaleUpPct   = 0.20f;
    public float scaleDownPct = 0.40f;
    public float shakePct     = 0.40f;

    float ScaleUpDur   => attackDuration * scaleUpPct;
    float ScaleDownDur => attackDuration * scaleDownPct;
    float ShakeDur     => attackDuration * shakePct;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public GameObject targetPos;
    public Vector3    bulletSpawnOffset    = new Vector3(0f, 4f, 0f);
    public float      bulletTravelDuration = 1.0f;
    public float      bulletArcHeight      = 2.0f;
    public float      bulletSpawnRotX      = 0f;

    [HorizontalLine(2f, 0.5f, 0.5f, 0.5f)]

    [Header("ANIMATION")]

    [Header("Anim Lid — Idle Wobble")]
    public GameObject lid;
    public float lidWobbleAmp  = 15f;
    public float lidWobbleFreq = 1.5f;
    public float lidLiftHeight = 0.1f;

    [Header("Anim Box — Float")]
    public GameObject box;
    public float boxFloatHeight = 0.2f;
    public float boxFloatFreq   = 0.5f;

    [Header("Anim Lid + Box — Attack")]
    public GameObject attackLid;
    public GameObject attackBox;
    public float attackScalePeak = 1.25f;
    public float attackShakeAmp  = 15f;



    void Start()
    {
        if (lid != null) StartCoroutine(LidWobbleLoop());
        if (box != null) StartCoroutine(BoxFloatLoop());
        StartCoroutine(AttackLoop());
    }

    IEnumerator LidWobbleLoop()
    {
        Vector3    basePos = lid.transform.localPosition;
        Quaternion baseRot = lid.transform.localRotation;

        while (true)
        {
            float t      = Time.time * lidWobbleFreq * Mathf.PI * 2f;
            float sin    = Mathf.Sin(t);
            float shaped = Mathf.Sign(sin) * Mathf.Pow(Mathf.Abs(sin), 0.6f);

            lid.transform.localRotation = baseRot * Quaternion.Euler(shaped * lidWobbleAmp, 0f, 0f);
            lid.transform.localPosition = basePos + new Vector3(0f, Mathf.Abs(sin) * lidLiftHeight, 0f);
            yield return null;
        }
    }

    IEnumerator BoxFloatLoop()
    {
        Vector3 originPos = box.transform.localPosition;
        Vector3 peakPos   = originPos + Vector3.up * boxFloatHeight;
        float   halfDur   = 0.5f / boxFloatFreq;

        while (true)
        {
            float t = 0f;
            while (t < halfDur) { t += Time.deltaTime; box.transform.localPosition = Vector3.Lerp(originPos, peakPos,   Mathf.SmoothStep(0f, 1f, t / halfDur)); yield return null; }
            t = 0f;
            while (t < halfDur) { t += Time.deltaTime; box.transform.localPosition = Vector3.Lerp(peakPos,   originPos, Mathf.SmoothStep(0f, 1f, t / halfDur)); yield return null; }
            box.transform.localPosition = originPos;
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalDuration);
            yield return StartCoroutine(AttackAnim());
        }
    }

    IEnumerator AttackAnim()
    {
        Vector3 boxBase = attackBox != null ? attackBox.transform.localScale : Vector3.one;
        Vector3 lidBase = attackLid != null ? attackLid.transform.localScale : Vector3.one;
        Vector3 boxPeak = boxBase * attackScalePeak;
        Vector3 lidPeak = lidBase * attackScalePeak;

        // Phase 1: scale up
        float t = 0f;
        while (t < ScaleUpDur)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / ScaleUpDur);
            if (attackBox != null) attackBox.transform.localScale = Vector3.Lerp(boxBase, boxPeak, p);
            if (attackLid != null) attackLid.transform.localScale = Vector3.Lerp(lidBase, lidPeak, p);
            yield return null;
        }
        SpawnBullet();

        // Phase 2: scale down
        t = 0f;
        while (t < ScaleDownDur)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / ScaleDownDur);
            if (attackBox != null) attackBox.transform.localScale = Vector3.Lerp(boxPeak, boxBase, p);
            if (attackLid != null) attackLid.transform.localScale = Vector3.Lerp(lidPeak, lidBase, p);
            yield return null;
        }
        if (attackBox != null) attackBox.transform.localScale = boxBase;
        if (attackLid != null) attackLid.transform.localScale = lidBase;

        // Phase 3: Z rotation shake, fades out
        Quaternion boxBaseRot = attackBox != null ? attackBox.transform.localRotation : Quaternion.identity;
        Quaternion lidBaseRot = attackLid != null ? attackLid.transform.localRotation : Quaternion.identity;
        t = 0f;
        while (t < ShakeDur)
        {
            t += Time.deltaTime;
            float angle = Mathf.Sin(t * 40f) * attackShakeAmp * (1f - t / ShakeDur);
            if (attackBox != null) attackBox.transform.localRotation = boxBaseRot * Quaternion.Euler(0f, 0f, angle);
            if (attackLid != null) attackLid.transform.localRotation = lidBaseRot * Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        if (attackBox != null) attackBox.transform.localRotation = boxBaseRot;
        if (attackLid != null) attackLid.transform.localRotation = lidBaseRot;
    }

    void SpawnBullet()
    {
        if (bulletPrefab == null || targetPos == null) return;
        Vector3 start = transform.TransformPoint(bulletSpawnOffset);
        GameObject bullet = Instantiate(bulletPrefab, start, Quaternion.Euler(bulletSpawnRotX, 0f, 0f));
        StartCoroutine(BulletTravel(bullet, start));
    }

    IEnumerator BulletTravel(GameObject bullet, Vector3 start)
    {
        Vector3 end     = targetPos.transform.position;
        float   elapsed = 0f;
        while (elapsed < bulletTravelDuration)
        {
            if (bullet == null) yield break;
            elapsed += Time.deltaTime;
            float   p   = Mathf.Clamp01(elapsed / bulletTravelDuration);
            Vector3 pos = Vector3.Lerp(start, end, p);
            pos.y += bulletArcHeight * 4f * p * (1f - p);
            bullet.transform.position = pos;
            yield return null;
        }
        if (bullet != null) Destroy(bullet);
    }
}
