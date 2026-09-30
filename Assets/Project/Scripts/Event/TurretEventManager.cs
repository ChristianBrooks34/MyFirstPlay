using System;

public class TurretEventManager : UnitEventManager
{
    public event Action OnStartAttack;
    public event Action OnCancelAttack;
    public event Action OnDisposable;
    public event Action OnItitialized;

    public void TriggerStartedAttack() => OnStartAttack?.Invoke();

    public void TriggerCanceledAttack() => OnCancelAttack?.Invoke();

    public void TriggerDisposable() => OnDisposable?.Invoke();

    public void TriggerInitialize() => OnItitialized?.Invoke();
}
