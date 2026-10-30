using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float waterLevel = 0f;
    public float floatStrength = 15f;
    public float waterDrag = 1f;
    public float waterAngularDrag = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {
            float displacementMultiplier = Mathf.Clamp01((waterLevel - transform.position.y) / 1f) * floatStrength;
            rb.AddForceAtPosition(Vector3.up * displacementMultiplier, transform.position, ForceMode.Acceleration);

            rb.linearDamping = waterDrag;
            rb.angularDamping = waterAngularDrag;
        }
        else
        {
            rb.linearDamping = 0.05f;
            rb.angularDamping = 0.05f;
        }
    }
}