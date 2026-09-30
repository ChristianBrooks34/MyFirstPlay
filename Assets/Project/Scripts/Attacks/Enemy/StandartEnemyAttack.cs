using UnityEngine;
using Zenject;

public class StandartEnemyAttack : BaseAttack
{
    private Enemy _enemy;
    private EnemyEventManager _enemyEventManager;

    [Inject]
    public void Construct(Enemy enemy, EnemyEventManager enemyEventManager)
    {
        _enemy = enemy;
        _enemyEventManager = enemyEventManager;

        enemyEventManager.OnStartAttack += OnHit;
    }

    public override void OnHit(GameObject gameObject)
    {
        if (!CanAttack()) return;

        if (gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            var knockbackDirection = (gameObject.transform.position - _enemy.transform.position).normalized;

            KnockbackControoller.Knockback(_enemy.Target, knockbackDirection);

            damageable.TryApplyDamage(_enemy.CalculateTotalDamage());

            _enemyEventManager.TriggerCanceledAttack();
        }

        //AttackLoop(AttackData.Cooldown).Forget();
    }

    private bool CanAttack()
    {
        return
            base.CanAttack &&
            _enemy.CanAttack &&
            _enemy.Target != null &&
            _enemy.Target.transform != null &&
            _enemy.IsDead == false;
    }
}
