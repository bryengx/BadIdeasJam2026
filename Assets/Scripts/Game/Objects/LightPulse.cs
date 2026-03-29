using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightPulse : MonoBehaviour
{
    [SerializeField] private float pulsePercent = 0.1f;
    [SerializeField] private float pulseDuration = 5f;

    private Light2D light2D;
    private float baseIntensity;

    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();

        if (light2D == null)
        {
            enabled = false;
            return;
        }

        baseIntensity = light2D.intensity;
    }

    private void Update()
    {
        float amplitude = baseIntensity * pulsePercent;

        float pulse = Mathf.Sin(Time.time * Mathf.PI * 2f / pulseDuration);

        light2D.intensity = baseIntensity + pulse * amplitude;
    }
}
