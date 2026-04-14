using System.Collections;
using UnityEngine;

public class Anim_Coffee : MonoBehaviour
{
    [Header("ATTACK TIMING (total = attack + interval)")]
    public float attackDuration   = 1.00f;
    public float intervalDuration = 2.00f;

    [Header("Float")]
    public GameObject plate;
    public GameObject cup;
    public float floatDistance = 0.3f;
    public float cupExtraFloat = 0.15f;
    public float floatSpeed    = 1.2f;
    public float cupPhaseDelay = 0.2f;

    [Header("Rotate")]
    public float rockAngle = 15f;
    public float rockSpeed = 1.0f;

    [Header("Attack")]
    public GameObject attackTarget;
    public float shakeIntensity  = 0.1f;

    private Vector3    plateBasePos;
    private Vector3    cupBasePos;
    private Quaternion cupBaseRot;
    private Vector3    cupBaseScale;
    private float      animTime;

    void Start()
    {
        if (plate) plateBasePos  = plate.transform.position;
        if (cup)
        {
            cupBasePos   = cup.transform.position;
            cupBaseRot   = cup.transform.localRotation;
            cupBaseScale = cup.transform.localScale;
        }
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return StartCoroutine(IntervalAnim());
            yield return StartCoroutine(AttackAnim());
        }
    }

    IEnumerator IntervalAnim()
    {
        float freq = floatSpeed * Mathf.PI * 2f;
        float end  = animTime + intervalDuration;
        while (animTime < end)
        {
            animTime += Time.deltaTime;
            if (plate) plate.transform.position    = plateBasePos + Vector3.up * (Mathf.Sin(animTime * freq) + 1f) * 0.5f * floatDistance;
            if (cup)   cup.transform.position      = cupBasePos   + Vector3.up * (Mathf.Sin((animTime - cupPhaseDelay) * freq) + 1f) * 0.5f * (floatDistance + cupExtraFloat);
            if (cup)   cup.transform.localRotation = cupBaseRot   * Quaternion.Euler(0f, Mathf.Sin(animTime * rockSpeed * Mathf.PI * 2f) * rockAngle, 0f);
            yield return null;
        }
    }

    IEnumerator AttackAnim()
    {
        if (!attackTarget) { yield return new WaitForSeconds(attackDuration); yield break; }

        Vector3 basePos   = attackTarget.transform.position;
        Vector3 baseScale = attackTarget.transform.localScale;

        for (float t = 0f; t < attackDuration; t += Time.deltaTime)
        {
            float p = t / attackDuration;
            attackTarget.transform.localScale = baseScale * (1f + Mathf.Sin(p * Mathf.PI) * 0.2f);
            attackTarget.transform.position   = basePos + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * shakeIntensity;
            yield return null;
        }

        attackTarget.transform.position   = basePos;
        attackTarget.transform.localScale = baseScale;
    }
}
