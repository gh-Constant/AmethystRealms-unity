using FishNet.Object;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : NetworkBehaviour
{
    public UIDocument uiDocument;

    private PlayerStats stats;
    private VisualElement healthFill;
    private VisualElement staminaFill;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            return;
        }

        stats = GetComponent<PlayerStats>();

        var root = uiDocument.rootVisualElement;

        // Find UI elements by name
        healthFill = root.Q<VisualElement>("healthFill");
        staminaFill = root.Q<VisualElement>("staminaFill");

        if (healthFill == null) Debug.LogError("healthFill NOT FOUND");
        if (staminaFill == null) Debug.LogError("staminaFill NOT FOUND");

        // Initialize
        UpdateHealth(stats.Health.Value);
        UpdateStamina(stats.Stamina.Value);

        // Subscribe to SyncVar update callbacks
        stats.Health.OnChange += (oldVal, newVal, asServer) => UpdateHealth(newVal);
        stats.Stamina.OnChange += (oldVal, newVal, asServer) => UpdateStamina(newVal);
    }

    private void UpdateHealth(float value)
    {
        float pct = value / stats.MaxHealth;
        healthFill.style.width = new Length(pct * 100f, LengthUnit.Percent);
    }

    private void UpdateStamina(float value)
    {
        float pct = value / stats.MaxStamina;
        staminaFill.style.width = new Length(pct * 100f, LengthUnit.Percent);
    }
}
