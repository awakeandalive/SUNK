using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    public float scrollSpeedX = 0.005f; // Slow SPEED
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
            // Shift texture clouds 
            float offsetX = Time.time * scrollSpeedX;
            float offsetZ = Time.time * scrollSpeedZ;
            cloudMaterial.mainTextureOffset = new Vector2(offsetX, offsetZ);
        }
    }
}