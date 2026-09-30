using System;
using UnityEngine;

public class Health
{
    private float _currentValue;
    private float _maxValue;

    public event Action<float> OnChangeHealth;

    public Health(UnitData unitData)
    {
        _maxValue = unitData.Health.CurrentValue;
        _currentValue = _maxValue;
    }

    public Health(UnitProfile unitProfile)
    {
        _maxValue = unitProfile.BaseData.Health.StartValue;
        _currentValue = _maxValue;
    }

    public Health(BuildingProfile buildingProfile)
    {
        _maxValue = buildingProfile.BaseData.Health.StartValue;
        _currentValue = _maxValue;
    }

    public void Initialize(float currentHealth)
    {
        _currentValue = currentHealth;
        OnChangeHealth?.Invoke(currentHealth);
    }

    public void Add(float value)
    {
        if (value < 0) return;

        _currentValue = Mathf.Min(_currentValue + value, _maxValue);
        OnChangeHealth?.Invoke(_currentValue);
    }

    public void Substract(float value)
    {
        if (value < 0) return;

        _currentValue = Mathf.Max(_currentValue - value, 0);
        OnChangeHealth?.Invoke(_currentValue);
    }

    public float CurrentValue => _currentValue;
    public float MaxValue => _maxValue;
}
