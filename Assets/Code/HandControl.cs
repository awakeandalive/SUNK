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
        // lclick punch left hand 3d
        if (Input.GetMouseButtonDown(0))
        {
            if (leftHandMelee != null)
            {
                leftHandMelee.UseItem();
            }
        }

        // rclick -> usable item
        if (Input.GetMouseButtonDown(1))
        {
            if (rightHandTool != null)
            {
                rightHandTool.UseItem();
            }
        }
    }
}