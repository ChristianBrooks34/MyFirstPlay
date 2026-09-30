using System;
using UniRx;
using UnityEngine;

public class ValueBar : MonoBehaviour
{
    [NonSerialized] public Transform DisplayPoint;
    [NonSerialized] public float MaxValue;
    [NonSerialized] public float MinValue;
    [NonSerialized] public ReactiveProperty<float> CurrentValue;

    public bool IsDisplayText;
    public bool IsValueBarVisible;

    public void Add(float value)
    {
        if (CurrentValue == null) CurrentValue = new ReactiveProperty<float>();
        if (value < 0)
        {
            Debug.LogError("Параметр value не может быть меньше нуля");
            return;
        }
        if (CurrentValue.Value + value > MaxValue)
        {
            CurrentValue.Value = MaxValue;
        }
        else CurrentValue.Value += value;
    }

    public void Substract(float value)
    {
        if (CurrentValue == null) CurrentValue = new ReactiveProperty<float>();
        if (value < 0)
        {
            Debug.LogError("Параметр value не может быть меньше нуля");
            return;
        }
        if (CurrentValue.Value - value < MinValue)
        {
            CurrentValue.Value = MinValue;
        }
        else CurrentValue.Value -= value;
    }
}
