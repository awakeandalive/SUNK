using UnityEngine;

public class PlayerLeftClick : MonoBehaviour
{
    [Header("Item Slot")]
    [Tooltip("Drag the item currently in your hand here")]
    public EquippableItem activeItem;

    void Update()
    {
        // what u have in left hand if weap use it if not punch
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
            // if no weapons show this mess (change later)
            Debug.Log("Swung empty hand / Punched!");
        }
    }
}