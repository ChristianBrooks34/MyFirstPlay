using System;

public class UnitEventManager
{
    public event Action OnDead;
    public event Action<Unit> OnDeadUnit;
    public event Action OnIdle;
    public event Action<float> OnDamage;
    public event Action<Unit> OnSpawned;

    public void TriggerDead(Unit unit)
    {
        OnDeadUnit?.Invoke(unit);
        OnDead?.Invoke();
    }

    public void TriggerDamage(float damage) => OnDamage?.Invoke(damage);

    public void TriggerIdle() => OnIdle?.Invoke();

    public void TriggerSpawn(Unit unit) => OnSpawned?.Invoke(unit);
}
