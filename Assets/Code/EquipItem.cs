using System.Collections;
using UnityEngine;

public class EquippableItem : MonoBehaviour
{
    [Header("Item Details")]
    public string itemName = "Right Hand Weapon";

    [Header("Swing / Attack Settings")]
    public float punchDistance = 0.3f;
    public float punchSpeed = 15f;
    public float returnSpeed = 10f;

    private Vector3 originalPosition;
    private bool isAttacking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public virtual void UseItem()
    {
        if (!isAttacking)
        {
            StartCoroutine(PunchRoutine());
        }
    }

    private IEnumerator PunchRoutine()
    {
        isAttacking = true;

        // Based on camera punch
        Vector3 targetPosition = originalPosition + new Vector3(0, 0, punchDistance);

        // Punch
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * punchSpeed);
            yield return null;
        }

        // Return back
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }

        transform.localPosition = originalPosition;
        isAttacking = false;
    }
}