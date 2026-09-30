using System;
using Zenject;

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Dead
}

public class EnemyStateMachine : IStateMachine, IInitializable, IDisposable
{
    private EnemyEventManager _enemyEventManager;
    public EnemyState CurrentState { get; protected set; }

    object IStateMachine.CurrentState => CurrentState;

    public EnemyStateMachine(EnemyEventManager enemyEventManager)
    {
        _enemyEventManager = enemyEventManager;
    }

    public void Initialize()
    {
        _enemyEventManager.OnStartAttack += (x) => ChangeState(EnemyState.Attack);
        _enemyEventManager.OnMove += () => ChangeState(EnemyState.Run);
        _enemyEventManager.OnDead += () => ChangeState(EnemyState.Dead);
    }

    public void ChangeState(EnemyState enemyState)
    {
        if (CurrentState == enemyState) return;
        CurrentState = enemyState;
    }

    public void Dispose()
    {
        _enemyEventManager.OnStartAttack -= (x) => ChangeState(EnemyState.Attack);
        _enemyEventManager.OnMove -= () => ChangeState(EnemyState.Run);
        _enemyEventManager.OnDead -= () => ChangeState(EnemyState.Dead);
    }

    public void ChangeState(object state)
    {
        ChangeState((EnemyState)state);
    }

    public bool IsCurrentState(object state)
    {
        return CurrentState.Equals((EnemyState)state);
    }
}
