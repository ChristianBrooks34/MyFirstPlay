using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : ValueBar
{
    private GlobalEventManager _globalEventManager;
    private Text _displayText;
    private DisplayValueBar _displayValueBar;
    private IDamageable _unit;

    [Inject]
    public void Construct(DisplayValueBar displayValueBar, Text textMeshProUGUI, GlobalEventManager globalEventManager)
    {
        _displayValueBar = displayValueBar;
        _displayText = textMeshProUGUI;
        _globalEventManager = globalEventManager;
    }

    public virtual void Initialize(IDamageable unit, Health health)
    {
        if (unit == null || health == null)
        {
            Debug.LogError("unit = null || health = null");
            return;
        }
        
        _unit = unit;
        MaxValue = health.CurrentValue;
        _displayValueBar.TextCount = _displayText;

        health.OnChangeHealth += UpdateHealth;
        _globalEventManager.OnChangedPlayerLevel += OnPlayerLevelChanged;

        UpdateHealth(health.CurrentValue);
        gameObject.SetActive(true);
    }

    private void UpdateHealth(float currentHealth)
    {
        _displayValueBar.Display(currentHealth, _unit.Health.MaxValue);
    }

    private void OnPlayerLevelChanged()
    {
        if (_unit == null)
        {
            Debug.LogError("_unit == null");
            return;
        }

        if (_unit.Health == null)
        {
            Debug.LogError("_unit.Health == null");
            return;
        }

        UpdateHealth(_unit.Health.CurrentValue);
    }

    public void OnDestroy()
    {
        if (_unit?.Health != null)
            _unit.Health.OnChangeHealth -= UpdateHealth;

        if (_globalEventManager != null)
        {
            _globalEventManager.OnChangedPlayerLevel -= OnPlayerLevelChanged;
        }
    }
}
