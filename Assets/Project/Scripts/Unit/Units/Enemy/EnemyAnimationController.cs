using UnityEngine;
using Zenject;

public class EnemyAnimationController : AnimationController, ITickable
{
    private readonly BaseMove _enemyMove;
    private readonly Animator _animator;
    private readonly FlipController _spriteController;
    private readonly Enemy _enemy;
    private readonly UnitEventManager _enemyEventManager;
    private readonly EnemyStateMachine _enemyStateMachine;

    public EnemyAnimationController(Enemy enemy, FlipController spriteController, BaseMove moveEnemy,
        Animator animator, UnitEventManager enemyEventManager, EnemyStateMachine enemyStateMachine)
    {
        _enemyStateMachine = enemyStateMachine;
        _enemyEventManager = enemyEventManager;
        _spriteController = spriteController;
        _enemyMove = moveEnemy;
        _animator = animator;
        _enemy = enemy;

        _enemyEventManager.OnDead += DeadAnimation;
    }

    public void Tick()
    {
        MoveAnimation();
    }

    private void MoveAnimation()
    {
        if (_enemyStateMachine.CurrentState == EnemyState.Dead) return;

        if (_enemyStateMachine.CurrentState == EnemyState.Dead)
            return;

        if (_spriteController == null)
        {
            Debug.LogWarning("_spriteController is null in MoveAnimation!");
            return;
        }

        if (_animator == null)
        {
            Debug.LogWarning("_animator is null in MoveAnimation!");
            return;
        }

        if (_enemyMove == null)
        {
            Debug.LogWarning("_enemyMove is null in MoveAnimation!");
            return;
        }

        if (_enemy == null)
        {
            Debug.LogWarning("_enemy is null in MoveAnimation!");
            return;
        }

        ControlMoveAnimation(
            _spriteController,
            _animator,
            _enemyMove,
            _enemy.transform);
    }

    private void DeadAnimation()
    {
        ControlDeadAnimation(_animator);
    }
}
