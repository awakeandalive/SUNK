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
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            waterMaterial = rend.material;
        }
    }

    void Update()
    {
        // up down float motion ocean waves
        float newY = startPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 2. Scroll the material texture to simulate moving water currents (starter ai change later for hyper realistic ocean waves)
        if (waterMaterial != null)
        {
            float offset = Time.time * textureScrollSpeed;
            waterMaterial.mainTextureOffset = new Vector2(offset, offset);
        }
    }
}