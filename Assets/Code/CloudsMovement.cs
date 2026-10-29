using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    public float scrollSpeedX = 0.005f; // Slow, eerie drift speed
    public float scrollSpeedZ = 0.003f;

    private Material cloudMaterial;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            cloudMaterial = rend.material;
        }
    }

    void Update()
    {
        if (cloudMaterial != null)
        {
            // Slowly shift the cloud texture offset over time
            float offsetX = Time.time * scrollSpeedX;
            float offsetZ = Time.time * scrollSpeedZ;
            cloudMaterial.mainTextureOffset = new Vector2(offsetX, offsetZ);
        }
    }
}