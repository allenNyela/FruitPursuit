using System.Collections;
using UnityEngine;

public class Anim_Clock : MonoBehaviour
{
    [Header("ATTACK PARAMETERS")]
    [Header("Timing")]
    public float attackPortion = 0.33f;
    private float intervalDuration;
    private float attackDuration;

    [Header("Bullet")]
    public GameObject bullet_Prefab;
    public Vector3 bulletSpawnOffset = new Vector3(0f, 5f, 5f);
    public float bulletLifetime = 1f;

    [HorizontalLine(2f, 0.5f, 0.5f, 0.5f)]

    [Header("ANIMATION")]

    [Header("Anim Clock Turns")]
    public GameObject minutePrefab;
    public GameObject hourPrefab;
    public float minuteSpeed = 6f;
    public float hourSpeed = 0.5f;

    [Header("Anim Signal Emit")]
    public GameObject wave_Prefab_A;
    public GameObject wave_Prefab_B;
    public float emitDuration = 0.6f;
    public float emitDistance = 0.5f;
    public float emitOffset = 0.1f;

    [Header("Attack Shake")]
    public GameObject body_Prefab;
    public float shakeIntensity = 0.3f;

    [Header("Idle Float")]
    public float idleFloatStrength = 0.05f;
    public float idleFloatSpeed    = 1f;

    private Turret turret;

    void Start()
    {
        turret = GetComponent<Turret>() ?? GetComponentInParent<Turret>();
        float total = turret != null && turret.fireRate > 0f ? 1f / turret.fireRate : 1.5f;
        attackDuration   = total * Mathf.Clamp01(attackPortion);
        intervalDuration = total * (1f - Mathf.Clamp01(attackPortion));

        StartSignalEmit();
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        RotateClockHands();
    }

    void RotateClockHands()
    {
        if (minutePrefab != null)
            minutePrefab.transform.Rotate(0f, 0f, -minuteSpeed * Time.deltaTime, Space.Self);
        if (hourPrefab != null)
            hourPrefab.transform.Rotate(0f, 0f, -hourSpeed * Time.deltaTime, Space.Self);
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (turret != null && turret.target != null)
            {
                yield return StartCoroutine(ShakeBody());
                yield return new WaitForSeconds(intervalDuration);
            }
            else
            {
                yield return StartCoroutine(IdleContinuous());
            }
        }
    }

    IEnumerator IdleContinuous()
    {
        if (body_Prefab == null) { yield return null; yield break; }
        float startY = body_Prefab.transform.localPosition.y;
        while (turret == null || turret.target == null)
        {
            float y = startY + Mathf.Sin(Time.time * idleFloatSpeed * Mathf.PI) * idleFloatStrength;
            Vector3 p = body_Prefab.transform.localPosition;
            body_Prefab.transform.localPosition = new Vector3(p.x, y, p.z);
            yield return null;
        }
        Vector3 reset = body_Prefab.transform.localPosition;
        body_Prefab.transform.localPosition = new Vector3(reset.x, startY, reset.z);
    }

    IEnumerator ShakeBody()
    {
        if (body_Prefab == null) yield break;

        Vector3 originPos = body_Prefab.transform.localPosition;
        Vector3 joltPos   = originPos + body_Prefab.transform.right * shakeIntensity;
        float rushDur     = attackDuration * 0.25f;
        float snapDur     = attackDuration * 0.75f;
        float spawnMark   = attackDuration * 0.25f;
        bool  bulletSpawned = false;
        float elapsed     = 0f;

        float t = 0f;
        while (t < rushDur)
        {
            t += Time.deltaTime;
            elapsed += Time.deltaTime;
            if (!bulletSpawned && elapsed >= spawnMark) { SpawnBullet(); bulletSpawned = true; }
            body_Prefab.transform.localPosition = Vector3.Lerp(originPos, joltPos, Mathf.SmoothStep(0f, 1f, t / rushDur));
            yield return null;
        }

        t = 0f;
        while (t < snapDur)
        {
            t += Time.deltaTime;
            elapsed += Time.deltaTime;
            if (!bulletSpawned && elapsed >= spawnMark) { SpawnBullet(); bulletSpawned = true; }
            float progress = t / snapDur;
            float spring   = Mathf.Sin(progress * Mathf.PI * 5f) * (1f - progress);
            body_Prefab.transform.localPosition = Vector3.Lerp(joltPos, originPos, Mathf.SmoothStep(0f, 1f, progress))
                                                + body_Prefab.transform.right * (spring * shakeIntensity * 0.4f);
            yield return null;
        }

        body_Prefab.transform.localPosition = originPos;
    }

    void SpawnBullet()
    {
        if (bullet_Prefab == null) return;
        Vector3 spawnPos = transform.TransformPoint(bulletSpawnOffset);
        GameObject bullet = Instantiate(bullet_Prefab, spawnPos, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b == null || !b.followOnHit) Destroy(bullet, bulletLifetime);
    }

    void StartSignalEmit()
    {
        if (wave_Prefab_A != null) StartCoroutine(SignalLoop(wave_Prefab_A, 0f));
        if (wave_Prefab_B != null) StartCoroutine(SignalLoop(wave_Prefab_B, emitOffset));
    }

    IEnumerator SignalLoop(GameObject wave, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        Vector3 originPos = wave.transform.localPosition;
        Vector3 peakPos   = originPos + Vector3.down * emitDistance;
        float halfDur     = emitDuration * 0.5f;

        while (true)
        {
            float t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                wave.transform.localPosition = Vector3.Lerp(originPos, peakPos, Mathf.SmoothStep(0f, 1f, t / halfDur));
                yield return null;
            }
            t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                wave.transform.localPosition = Vector3.Lerp(peakPos, originPos, Mathf.SmoothStep(0f, 1f, t / halfDur));
                yield return null;
            }
            wave.transform.localPosition = originPos;
        }
    }
}
