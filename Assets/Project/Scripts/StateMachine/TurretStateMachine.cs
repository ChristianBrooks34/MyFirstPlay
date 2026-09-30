using System;
using Zenject;
public enum TurretState
{
    Idle,
    Attack,
    Dead
}

public class TurretStateMachine : IStateMachine, IInitializable, IDisposable
{
    private TurretEventManager _turretEvenManager;
    public TurretState CurrentState { get; protected set; }

    object IStateMachine.CurrentState => CurrentState;

    public TurretStateMachine(TurretEventManager turretEvenManager)
    {
        _turretEvenManager = turretEvenManager;
    }

    public void Initialize()
    {
        _turretEvenManager.OnStartAttack += () => ChangeState(TurretState.Attack);
        _turretEvenManager.OnDead += () => ChangeState(TurretState.Dead);
    }

    public void ChangeState(TurretState enemyState)
    {
        if (CurrentState == enemyState) return;
        CurrentState = enemyState;
    }

    public void Dispose()
    {
        _turretEvenManager.OnStartAttack -= () => ChangeState(TurretState.Attack);
        _turretEvenManager.OnDead -= () => ChangeState(TurretState.Dead);
    }

    void IStateMachine.ChangeState(object state)
    {
        ChangeState((TurretState)state);
    }

    public bool IsCurrentState(object state)
    {
        return CurrentState.Equals((EnemyState)state);
    }
}
