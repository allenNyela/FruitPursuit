using System.Collections;
using UnityEngine;

public class Anim_Clock : MonoBehaviour
{
    [Header("ATTACK PARAMETERS")]
    [Header("Timing")]
    public float intervalDuration  = 1f;    // seconds between each attack
    public float attackDuration  = 0.5f;  // seconds for one attack anim

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



    void Start()
    {
        StartSignalEmit();
        StartCoroutine(ShakeLoop());
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

    void StartSignalEmit()
    {
        if (wave_Prefab_A != null)
            StartCoroutine(SignalLoop(wave_Prefab_A, 0f));
        if (wave_Prefab_B != null)
            StartCoroutine(SignalLoop(wave_Prefab_B, emitOffset));
    }

    IEnumerator ShakeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalDuration);
            yield return StartCoroutine(ShakeBody());
        }
    }

    IEnumerator ShakeBody()
    {
        if (body_Prefab == null) yield break;

        Vector3 originPos = body_Prefab.transform.localPosition;
        Vector3 joltPos   = originPos + body_Prefab.transform.right * shakeIntensity;
        float rushDur     = attackDuration * 0.25f; // fast lunge forward
        float snapDur     = attackDuration * 0.75f; // damped spring back
        float spawnMark   = attackDuration * 0.25f;
        bool  bulletSpawned = false;
        float elapsed     = 0f;

        // Rush forward
        float t = 0f;
        while (t < rushDur)
        {
            t += Time.deltaTime;
            elapsed += Time.deltaTime;
            if (!bulletSpawned && elapsed >= spawnMark) { SpawnBullet(); bulletSpawned = true; }
            float ease = Mathf.SmoothStep(0f, 1f, t / rushDur);
            body_Prefab.transform.localPosition = Vector3.Lerp(originPos, joltPos, ease);
            yield return null;
        }

        // Spring back with damped oscillation
        t = 0f;
        while (t < snapDur)
        {
            t += Time.deltaTime;
            elapsed += Time.deltaTime;
            if (!bulletSpawned && elapsed >= spawnMark) { SpawnBullet(); bulletSpawned = true; }
            float progress = t / snapDur;
            float eased    = Mathf.SmoothStep(0f, 1f, progress);
            float spring   = Mathf.Sin(progress * Mathf.PI * 5f) * (1f - progress);
            body_Prefab.transform.localPosition = Vector3.Lerp(joltPos, originPos, eased)
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
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator SignalLoop(GameObject wave, float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 originPos = wave.transform.localPosition;
        Vector3 peakPos   = originPos + Vector3.down * emitDistance;
        float halfDur     = emitDuration * 0.5f;

        while (true)
        {
            // Move down
            float t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                float ease = Mathf.SmoothStep(0f, 1f, t / halfDur);
                wave.transform.localPosition = Vector3.Lerp(originPos, peakPos, ease);
                yield return null;
            }

            // Move back up
            t = 0f;
            while (t < halfDur)
            {
                t += Time.deltaTime;
                float ease = Mathf.SmoothStep(0f, 1f, t / halfDur);
                wave.transform.localPosition = Vector3.Lerp(peakPos, originPos, ease);
                yield return null;
            }

            wave.transform.localPosition = originPos;

        }
    }
}
