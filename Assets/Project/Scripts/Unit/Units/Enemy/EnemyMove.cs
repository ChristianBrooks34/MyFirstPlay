using UnityEngine;
using Zenject;

public class EnemyMove : BaseMove
{
    private Enemy _enemy;
    private EnemyEventManager _enemyEventManager;
    private EnemyStateMachine _enemyStateMachine;
    private EnemyMoveContainer _enemyMoveContainer;

    [Inject]
    public void Construct(Enemy enemy, EnemyEventManager enemyEventManager, EnemyStateMachine enemyStateMachine,
        EnemyMoveContainer enemyMoveContainer)
    {
        _enemy = enemy;
        _enemyEventManager = enemyEventManager;
        _enemyStateMachine = enemyStateMachine;
        _enemyMoveContainer = enemyMoveContainer;

        if (!_enemy.IsEnemyMoveRegisteredInContainer)
        {
            _enemyMoveContainer.Add(this);
            _enemy.IsEnemyMoveRegisteredInContainer = true;
        }
    }

    public override void Move()
    {
        if (!CanMove()) return;

        Offset = GetMoveOffset();

        _enemy.transform.position += new Vector3(Offset.x, Offset.y, 0);

        _enemyEventManager.TriggerMove();
    }

    private Vector3 GetMoveOffset()
    {
        var direction = GetMoveDirectional(_enemy.Target.transform.position);

        //Debug.Log($"Speed {_enemy.name} = " + _enemy.GameData.SpeedMovement.CurrentValue);
        ChargeDirection(direction);
        return new Vector3(direction.normalized.x, direction.normalized.y, 0)
            * _enemy.UnitProfile.BaseData.SpeedMovement.CurrentValue * Time.deltaTime;
    }

    private Vector3 GetMoveDirectional(Vector3 target)
    {
        return target - _enemy.transform.position;
    }

    public bool CanMove()
    {
        return
            _enemyStateMachine.CurrentState != EnemyState.Dead &&
            !_enemy.IsDisposable &&
            !_enemy.IsDead &&
            _enemy.Target != null &&
            !_enemy.Target.IsDead;
    }

    public void OnDestroy()
    {
        _enemyMoveContainer.Remove(this);
    }
}

