using UnityEngine;

public class OceanWaves : MonoBehaviour
{
    public float waveSpeed = 0.5f;
    public float waveHeight = 0.3f;
    public float textureScrollSpeed = 0.05f;

    private Vector3 startPosition;
    private Material waterMaterial;

    void Start()
    {
        startPosition = transform.position;

        // Grab the material from the plane renderer so we can animate it
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            waterMaterial = rend.material;
        }
    }

    void Update()
    {
        // 1. Gentle up-and-down floating motion for the whole ocean surface
        float newY = startPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 2. Scroll the material texture to simulate moving water currents
        if (waterMaterial != null)
        {
            float offset = Time.time * textureScrollSpeed;
            waterMaterial.mainTextureOffset = new Vector2(offset, offset);
        }
    }
}