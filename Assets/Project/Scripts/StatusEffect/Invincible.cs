
using UnityEngine;

public class Invincible : IEffect
{
    public bool IsActive { get; private set; }

    private IDamageable _damagableObject;

    public Invincible(IDamageable damagableObject)
    {
        _damagableObject = damagableObject;
    }

    public void Apply()
    {
        if (IsActive) return;
        IsActive = true;
        _damagableObject.CanBeAttacked = false;
    }

    public void Remove()
    {
        _damagableObject.CanBeAttacked = true;
        IsActive = false;
    }
}

