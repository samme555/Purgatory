using UnityEngine;
using UnityEngine.Rendering.Universal;

// Adds a dynamic flickering effect to a 2D light
public class LightFlicker : MonoBehaviour
{
    private Light2D light2D; // Reference to the Light2D component

    public float flickerAmount = 0.2f; // Maximum intensity offset
    public float flickerSpeed = 10f; // Speed at which the flickering occurs

    private float baseIntensity; // Original light intensity baseline

    // Cache reference and intensity at start
    void Start()
    {
        light2D = GetComponent<Light2D>();
        baseIntensity = light2D.intensity;
    }

    // Update intensity using Perlin noise for smooth random flicker
    void Update()
    {
        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerAmount;
        light2D.intensity = baseIntensity + flicker;
    }
}