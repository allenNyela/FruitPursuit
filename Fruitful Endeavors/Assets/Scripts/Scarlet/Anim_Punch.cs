using System.Collections;
using UnityEngine;

public class Anim_Punch : MonoBehaviour
{
    [Header("ATTACK TIMING (total = attack + interval)")]
    public float attackDuration   = 1.00f;
    public float intervalDuration = 1.50f;

    [Header("Arm")]
    public GameObject arm;
    public float punchScaleIntensity = 0.4f;
    public float punchAnimDuration   = 0.25f;

    [Header("Fist")]
    public GameObject fist;
    public float      fistSpeed         = 20f;
    public float      fistTravelDistance = 10f;
    public float      fistSpawnAnimDur  = 0.2f;

    private GameObject fistTemplate;  
    private Vector3    fistBasePos;
    private Quaternion fistBaseRot;
    private Vector3    fistBaseScale;

    void Start()
    {
        if (fist != null)
        {
            fistBasePos   = fist.transform.position;
            fistBaseRot   = fist.transform.rotation;
            fistBaseScale = fist.transform.lossyScale;

            fistTemplate = Instantiate(fist);
            fistTemplate.SetActive(false);
        }
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalDuration);

            if (fist != null) StartCoroutine(LaunchFist());
            if (arm != null)  yield return StartCoroutine(PunchAnim());

            yield return new WaitForSeconds(intervalDuration * 0.5f);
            if (fistTemplate != null)
            {
                fist = Instantiate(fistTemplate, fistBasePos, fistBaseRot);
                fist.SetActive(true);
                StartCoroutine(SpawnScaleAnim(fist));
            }

            yield return new WaitForSeconds(intervalDuration * 0.5f);
        }
    }

    IEnumerator LaunchFist()
    {
        GameObject instance = fist;
        fist = null;  // turret 上不再持有引用

        Vector3 direction = Vector3.forward;
        float traveled = 0f;

        while (traveled < fistTravelDistance)
        {
            if (instance == null) yield break;
            float step = fistSpeed * Time.deltaTime;
            instance.transform.position += direction * step;
            traveled += step;
            yield return null;
        }

        if (instance != null) Destroy(instance);
    }

    IEnumerator SpawnScaleAnim(GameObject target)
    {
        float t = 0f;
        while (t < fistSpawnAnimDur)
        {
            if (target == null) yield break;
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / fistSpawnAnimDur);
            target.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, fistBaseScale, p);
            yield return null;
        }
        target.transform.localScale = fistBaseScale;
    }

    IEnumerator PunchAnim()
    {
        Vector3 baseScale = arm.transform.localScale;
        Vector3 peakScale = baseScale * (1f + punchScaleIntensity);
        float half = punchAnimDuration * 0.5f;

        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            arm.transform.localScale = Vector3.Lerp(baseScale, peakScale, Mathf.SmoothStep(0f, 1f, t / half));
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            arm.transform.localScale = Vector3.Lerp(peakScale, baseScale, Mathf.SmoothStep(0f, 1f, t / half));
            yield return null;
        }

        arm.transform.localScale = baseScale;
    }
}
