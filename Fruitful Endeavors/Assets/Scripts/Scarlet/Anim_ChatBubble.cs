using System.Collections;
using UnityEngine;

public class Anim_ChatBubble : MonoBehaviour
{
    [Header("TIMING (total=attack+interval)")]

    public float attackDuration   = 1.50f;  // head lift → pop → spring → snap
    public float intervalDuration = 1.50f;  // idle jelly between attacks

    [Header("Attack Sub-phases (% of attackDuration)")]
    public float liftPct   = 0.50f;
    public float popUpPct  = 0.05f;
    public float springPct = 0.20f;
    public float snapPct   = 0.25f;

    float LiftDur   => attackDuration * liftPct;
    float PopUpDur  => attackDuration * popUpPct;
    float SpringDur => attackDuration * springPct;
    float SnapDur   => attackDuration * snapPct;

    [HorizontalLine(2f, 0.5f, 0.5f, 0.5f)]

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Vector3    bulletSpawnOffset = new Vector3(0f, 4f, 8f);
    public float      bulletLifetime    = 0.8f;

    [HorizontalLine(2f, 0.5f, 0.5f, 0.5f)]

    [Header("ANIMATION")]

    [Header("Anim Head — Lift")]
    public GameObject head;
    public float headLiftAngle = 40f;

    [Header("Anim Head — Scale pop at peak")]
    public float headPopScale    = 1.3f;
    public float headSpringDecay = 8f;
    public float headSpringFreq  = 14f;

    [Header("Anim Head — Idle jelly & wobble")]
    public float headJellyAmp   = 0.05f;
    public float headJellyFreq  = 1.5f;
    public float headWobbleAmp  = 0.03f;
    public float headWobbleFreq = 0.6f;

    [Header("Anim Bubbles Float")]
    public GameObject bubble1;
    public GameObject bubble2;
    public float bubbleFloatHeight   = 0.35f;
    public float bubbleFloatDuration = 3.0f;
    public float bubblePhaseOffset   = 0.5f;

    [Header("Anim Bullet Spawn")]
    public float bulletScaleInDuration = 0.4f;  // how long the bullet grows after spawning
    public float bulletSpawnScale      = 0.1f;
    public float bulletFinalScale      = 1.0f;



    void Start()
    {
        if (bubble1 != null) StartCoroutine(BubbleFloatLoop(bubble1, 0f));
        if (bubble2 != null) StartCoroutine(BubbleFloatLoop(bubble2, bubblePhaseOffset));
        StartCoroutine(AttackLoop());
    }


    // ── Bubble float ─────────────────────────────────────────────────────────

    IEnumerator BubbleFloatLoop(GameObject bubble, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        Vector3 originPos = bubble.transform.localPosition;
        Vector3 peakPos   = originPos + Vector3.up * bubbleFloatHeight;
        float   halfDur   = bubbleFloatDuration * 0.5f;

        while (true)
        {
            float t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                bubble.transform.localPosition = Vector3.Lerp(originPos, peakPos, Mathf.SmoothStep(0f, 1f, t / halfDur));
                yield return null;
            }
            t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                bubble.transform.localPosition = Vector3.Lerp(peakPos, originPos, Mathf.SmoothStep(0f, 1f, t / halfDur));
                yield return null;
            }
            bubble.transform.localPosition = originPos;
        }
    }


    // ── Attack loop ──────────────────────────────────────────────────────────

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return StartCoroutine(HeadAttack());
            yield return StartCoroutine(HeadIdleJelly());
        }
    }

    IEnumerator HeadAttack()
    {
        if (head == null) yield break;

        Quaternion startRot   = head.transform.localRotation;
        Quaternion peakRot    = startRot * Quaternion.Euler(-headLiftAngle, 0f, 0f);
        Vector3    startPos   = head.transform.localPosition;
        Vector3    startScale = head.transform.localScale;

        // Phase 1: Slow lift (ease-in)
        float t = 0f;
        while (t < LiftDur)
        {
            t += Time.deltaTime;
            float p = t / LiftDur;
            head.transform.localRotation = Quaternion.Lerp(startRot, peakRot, p * p);
            yield return null;
        }
        head.transform.localRotation = peakRot;
        head.transform.localPosition = startPos;
        SpawnBullet();

        // Phase 2: Quick scale pop up
        Vector3 peakScale = startScale * headPopScale;
        t = 0f;
        while (t < PopUpDur)
        {
            t += Time.deltaTime;
            head.transform.localScale = Vector3.Lerp(startScale, peakScale, Mathf.SmoothStep(0f, 1f, t / PopUpDur));
            yield return null;
        }

        // Phase 3: Damped spring on scale
        t = 0f;
        while (t < SpringDur)
        {
            t += Time.deltaTime;
            float spring = 1f + (headPopScale - 1f) * Mathf.Exp(-headSpringDecay * t) * Mathf.Cos(headSpringFreq * t);
            head.transform.localScale = startScale * spring;
            yield return null;
        }
        head.transform.localScale = startScale;

        // Phase 4: Fast snap back
        t = 0f;
        while (t < SnapDur)
        {
            t += Time.deltaTime;
            head.transform.localRotation = Quaternion.Lerp(peakRot, startRot, Mathf.SmoothStep(0f, 1f, t / SnapDur));
            yield return null;
        }
        head.transform.localRotation = startRot;
    }


    // ── Idle jelly ───────────────────────────────────────────────────────────

    IEnumerator HeadIdleJelly()
    {
        if (head == null) { yield return new WaitForSeconds(intervalDuration); yield break; }

        Vector3 startScale = head.transform.localScale;
        Vector3 startPos   = head.transform.localPosition;
        float   fadeDur    = Mathf.Min(0.25f, intervalDuration * 0.25f);
        float   elapsed    = 0f;

        while (elapsed < intervalDuration)
        {
            elapsed += Time.deltaTime;

            float env = elapsed < fadeDur                    ? elapsed / fadeDur :
                        elapsed > intervalDuration - fadeDur ? (intervalDuration - elapsed) / fadeDur : 1f;

            float jelly    = Mathf.Sin(elapsed * headJellyFreq * Mathf.PI * 2f) * headJellyAmp * env;
            float yFactor  = 1f + jelly;
            float xzFactor = 1f / Mathf.Sqrt(Mathf.Max(yFactor, 0.01f));
            head.transform.localScale = new Vector3(startScale.x * xzFactor, startScale.y * yFactor, startScale.z * xzFactor);

            float wobble = Mathf.Sin(elapsed * headWobbleFreq * Mathf.PI * 2f) * headWobbleAmp * env;
            head.transform.localPosition = startPos + Vector3.up * wobble;

            yield return null;
        }

        head.transform.localScale    = startScale;
        head.transform.localPosition = startPos;
    }


    // ── Bullet ───────────────────────────────────────────────────────────────

    void SpawnBullet()
    {
        if (bulletPrefab == null) return;
        GameObject bullet = Instantiate(bulletPrefab, transform.TransformPoint(bulletSpawnOffset), Quaternion.identity);
        Destroy(bullet, bulletLifetime);
        StartCoroutine(ScaleIn(bullet, bulletScaleInDuration));
    }

    IEnumerator ScaleIn(GameObject obj, float duration)
    {
        Vector3 baseScale = obj.transform.localScale;
        Vector3 fromScale = baseScale * bulletSpawnScale;
        Vector3 toScale   = baseScale * bulletFinalScale;
        obj.transform.localScale = fromScale;

        float t = 0f;
        while (t < duration)
        {
            if (obj == null) yield break;
            t += Time.deltaTime;
            float p     = Mathf.Clamp01(t / duration);
            float eased = p < 0.7f
                ? Mathf.Pow(p / 0.7f, 2.5f) * 0.25f
                : 0.25f + 0.75f * Mathf.Sqrt((p - 0.7f) / 0.3f);
            obj.transform.localScale = Vector3.Lerp(fromScale, toScale, eased);
            yield return null;
        }

        if (obj != null) obj.transform.localScale = toScale;
    }
}
