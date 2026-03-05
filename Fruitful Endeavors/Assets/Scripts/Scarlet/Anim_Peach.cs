using UnityEngine;
using System.Collections;

public class Anim_Peach : MonoBehaviour
{
    [Header("Breathe")]
    public float breatheIntensity = 0.08f;
    public float breatheSpeed = 1.2f;

    [Header("Hit")]
    public float shakeIntensity = 0.06f;
    public float zShakeIntensity = 0.15f;
    public float hitScaleIntensity = 0.2f;
    public float hitDuration = 0.3f;

    private Vector3 baseScale;
    private Vector3 baseLocalPos;
    private bool isHitting = false;

    private void Start()
    {
        baseScale = transform.localScale;
        baseLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (isHitting) return;

        // Jelly breathe: Y expands and contracts, XZ compensates slightly
        float wave = (Mathf.Sin(Time.time * breatheSpeed * Mathf.PI) + 1f) * 0.5f;
        float scaleY  = 1f + wave * breatheIntensity;
        float scaleXZ = 1f - wave * breatheIntensity * 0.4f;
        transform.localScale = new Vector3(
            baseScale.x * scaleXZ,
            baseScale.y * scaleY,
            baseScale.z * scaleXZ
        );
    }

    // Call this from Health.TakeDamage() or any external script
    public void PlayHitAnim()
    {
        if (!isHitting)
            StartCoroutine(HitCoroutine());
    }

    private IEnumerator HitCoroutine()
    {
        isHitting = true;
        float elapsed = 0f;

        while (elapsed < hitDuration)
        {
            float t = elapsed / hitDuration;
            float decay = 1f - t;

            // Shake position (XY random + Z oscillation)
            float shake = shakeIntensity * decay;
            float zShake = Mathf.Sin(elapsed * 80f) * zShakeIntensity * decay;
            transform.localPosition = baseLocalPos + new Vector3(
                Random.Range(-1f, 1f) * shake,
                Random.Range(-0.3f, 0.3f) * shake,
                zShake
            );

            // Scale pulse: Y stretches on hit, XZ compresses
            float pulse = Mathf.Sin(t * Mathf.PI) * hitScaleIntensity;
            transform.localScale = new Vector3(
                baseScale.x * (1f - pulse * 0.4f),
                baseScale.y * (1f + pulse),
                baseScale.z * (1f - pulse * 0.4f)
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = baseLocalPos;
        transform.localScale = baseScale;
        isHitting = false;
    }
}
