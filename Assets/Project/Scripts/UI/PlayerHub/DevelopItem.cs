using System;
using UnityEngine;

[System.Serializable]
public class DevelopItem<T> where T : struct, IConvertible
{
    [SerializeField] public int NumberDevelop;
    [SerializeField] public string Name;
    [SerializeField] public T CurrentValue;
    [SerializeField] public T StartValue;
    [SerializeField] public float IncrementValue;
    [SerializeField] public int Price;
    [SerializeField] public int StartPrice;
    [SerializeField] public float PriceMultiplayer;

    public void Develop()
    {
        Price = (int)Mathf.Round(Price * PriceMultiplayer);

        NumberDevelop++;

        CurrentValue = AddValues(CurrentValue, IncrementValue);
    }

    private T AddValues(T a, float b)
    {
        double valueA = Convert.ToDouble(a);
        double valueB = Convert.ToDouble(b);
        double result = valueA + valueB;

        return (T)Convert.ChangeType(result, typeof(T));
    }
}

[System.Serializable]
public class DevelopItemFloat : DevelopItem<float> { }
