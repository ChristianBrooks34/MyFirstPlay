using System;

public class BuildingEventManager
{
    public event Action OnInitialize;
    public event Action OnDestroy;
    public event Action<float> OnDamage;

    public void TriggetInitialize() => OnInitialize?.Invoke();
    public void TriggetDestroy() => OnDestroy?.Invoke();
    public void TriggetDamage(float damage) => OnDamage?.Invoke(damage);

}
