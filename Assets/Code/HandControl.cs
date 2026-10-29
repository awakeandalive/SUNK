using UnityEngine;

public class PlayerHandControls : MonoBehaviour
{
    [Header("Hand Items")]
    [Tooltip("Drag LeftHand capsule here (equipped with EquippableItem)")]
    public EquippableItem leftHandMelee;

    [Tooltip("Drag RightHand capsule here (equipped with FlashlightItem or EquippableItem)")]
    public EquippableItem rightHandTool;

    void Update()
    {
        // Left-Click (0) -> Melee / Punch with Left Hand
        if (Input.GetMouseButtonDown(0))
        {
            if (leftHandMelee != null)
            {
                leftHandMelee.UseItem();
            }
        }

        // Right-Click (1) -> Use Item / Toggle Light with Right Hand
        if (Input.GetMouseButtonDown(1))
        {
            if (rightHandTool != null)
            {
                rightHandTool.UseItem();
            }
        }
    }
}