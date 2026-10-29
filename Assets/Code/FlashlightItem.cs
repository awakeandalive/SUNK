using UnityEngine;

public class FlashlightItem : EquippableItem
{
    [Header("Flashlight Component")]
    public Light flashlightSpotLight;

    private bool isOn = false;

    void Start()
    {
        // Make sure light starts off if referenced
        if (flashlightSpotLight != null)
        {
            flashlightSpotLight.enabled = isOn;
        }
    }

    // Overrides UseItem so Right-Click toggles the light
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