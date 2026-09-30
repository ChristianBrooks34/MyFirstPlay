using System;
using UnityEngine;

public class Health
{
    private float _currentValue;
    private float _maxValue;

    public event Action<float> OnChangeHealth;

    public Health(UnitData unitData, bool initializeWithMax = true)
    {
        _maxValue = unitData.Health.StartValue;
        _currentValue = initializeWithMax ? _maxValue : unitData.Health.CurrentValue;
    }

    public Health(UnitProfile unitProfile, bool initializeWithMax = true)
    {
        _maxValue = unitProfile.BaseData.Health.StartValue;
        _currentValue = initializeWithMax ? _maxValue : unitProfile.BaseData.Health.CurrentValue;
    }

    public Health(BuildingProfile buildingProfile, bool initializeWithMax = true)
    {
        _maxValue = buildingProfile.BaseData.Health.StartValue;
        _currentValue = initializeWithMax ? _maxValue : buildingProfile.BaseData.Health.CurrentValue;
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
