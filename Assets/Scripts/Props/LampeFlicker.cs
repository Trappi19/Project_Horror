using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Lampe à contrôler")]
    public Light targetLight;   // Assigne ici ton Spot / Point Light
    public Light OnLight;   // Assigne ici ton Spot / Point Light


    [Header("Intensité")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.5f;

    [Header("Vitesse du flicker")]
    public float flickerSpeed = 1.5f;
    public float speedJitter = 0.7f;

    [Header("Options")]
    public float baseIntensity = 1.5f;
    [Range(0f, 5f)] public float flickerStrength = 1f;

    private float timeOffset;

    void Start()
    {
        timeOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (targetLight == null) return;

        float t = Time.time + timeOffset;
        float dynamicSpeed = flickerSpeed + Mathf.PerlinNoise(t * 0.1f, 0f) * speedJitter;
        float noise = Mathf.PerlinNoise(t * dynamicSpeed, t * 0.37f);

        float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        float finalIntensity = Mathf.Lerp(baseIntensity, targetIntensity, flickerStrength);

        targetLight.intensity = Mathf.Lerp(targetLight.intensity, finalIntensity, Time.deltaTime * 10f);
        OnLight.intensity = Mathf.Lerp(OnLight.intensity, finalIntensity, Time.deltaTime * 10f);
    }
}
