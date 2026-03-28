using UnityEngine;

public class LightBlink : MonoBehaviour
{
    public GameObject lightObject;
    public float minFrequency  = 1f;
    public float maxFrequency  = 4f;
    public float maxIntensity  = 2f;
    public float minIntensity  = 0.2f;
    public float transitionSpeed = 20f;

    private Light pointLight;
    private float blinkTimer;
    private float targetIntensity;

    private void Start()
    {
        if (lightObject != null)
            pointLight = lightObject.GetComponent<Light>();
        if (pointLight != null)
            targetIntensity = pointLight.intensity;
    }

    private void Update()
    {
        if (pointLight == null) return;

        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            blinkTimer = 1f / Random.Range(minFrequency, maxFrequency);
        }

        pointLight.intensity = Mathf.Lerp(pointLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);
    }
}
