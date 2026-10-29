using UnityEngine;

public class SeaPlantSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swaySpeed = 1.5f;
    public float swayAngle = 5f;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Gentle back-and-forth tilt imitating underwater currents
        float tilt = Mathf.Sin(Time.time * swaySpeed) * swayAngle;
        transform.localRotation = startRotation * Quaternion.Euler(tilt, 0, tilt * 0.5f);
    }
}