using UnityEngine;

public class FlashlightItem : EquippableItem
{
    [Header("Flashlight Component")]
    public Light flashlightSpotLight;

    private bool isOn = false;

    void Start()
    {
        // light must statt off
        if (flashlightSpotLight != null)
        {
            flashlightSpotLight.enabled = isOn;
        }
    }

    // Overrides UseItem --> Right-Click toggles light fix later (if i have anotheridea)
    public override void UseItem()
    {
        if (flashlightSpotLight != null)
        {
            isOn = !isOn;
            flashlightSpotLight.enabled = isOn;
            Debug.Log("Flashlight toggled: " + (isOn ? "ON" : "OFF"));
        }
        else
        {
            Debug.Log("Right hand item used (No Light component assigned)!");
        }
    }
}