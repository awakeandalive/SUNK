using UnityEngine;

public class UnderwaterEffect : MonoBehaviour
{
    [Header("Wave Matching Settings")]
    public Transform oceanTransform;
    public float waveHeight = 0.6f;
    public float waveSpeed = 2f;
    public float waveFrequency = 0.3f;

    [Header("Instant Responsiveness")]
    public float waterOffset = 0.25f; // A tiny buffer to eliminate the split-second gray flash!

    [Header("Colors")]
    public Color normalSkyColor = new Color(0.4f, 0.6f, 0.9f);
    public Color underwaterColor = new Color(0.02f, 0.15f, 0.35f);

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (oceanTransform == null)
        {
            GameObject oceanObj = GameObject.Find("Ocean");
            if (oceanObj != null)
            {
                oceanTransform = oceanObj.transform;
            }
        }
    }

    void Update()
    {
        if (oceanTransform == null) return;

        // Use relative position so the wave math matches the ocean mesh perfectly
        Vector3 localPos = transform.position - oceanTransform.position;
        float x = localPos.x;
        float z = localPos.z;

        // Exact same wave formula as your RealWaves script
        float wave1 = Mathf.Sin(Time.time * waveSpeed + (x * waveFrequency) + (z * waveFrequency));
        float wave2 = Mathf.Cos(Time.time * (waveSpeed * 0.7f) + (x * (waveFrequency * 1.3f) - (z * waveFrequency)));

        // Add the tiny waterOffset buffer so it reacts instantly on touch
        float currentWaterHeight = oceanTransform.position.y + ((wave1 + wave2) * waveHeight) + waterOffset;

        // Check if camera is below the buffered water height
        if (transform.position.y < currentWaterHeight)
        {
            // Under Water Settings
            RenderSettings.fog = true;
            RenderSettings.fogColor = underwaterColor;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.08f;

            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = underwaterColor;
        }
        else
        {
            // Above Water Settings (Clean reset)
            RenderSettings.fog = false;
            cam.clearFlags = CameraClearFlags.Skybox;
            cam.backgroundColor = normalSkyColor;
        }
    }
}