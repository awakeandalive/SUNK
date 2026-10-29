using UnityEngine;

public class PlayerLeftClick : MonoBehaviour
{
    [Header("Item Slot")]
    [Tooltip("Drag the item currently in your hand here")]
    public EquippableItem activeItem;

    void Update()
    {
        // Check for Left Mouse Button click (0 = Left Click)
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttackOrUse();
        }
    }

    void PerformAttackOrUse()
    {
        if (activeItem != null)
        {
            activeItem.UseItem();
        }
        else
        {
            // Empty-handed attack / punch
            Debug.Log("Swung empty hand / Punched!");
        }
    }
}