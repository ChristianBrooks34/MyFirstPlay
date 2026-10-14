using UnityEngine;
using Zenject;

public class StandartEnemyAttack : BaseAttack
{
    private Enemy _enemy;
    private EnemyEventManager _enemyEventManager;
    private bool _isSubscribed;

    [Inject]
    public void Construct(Enemy enemy, EnemyEventManager enemyEventManager)
    {
        _enemy = enemy;
        _enemyEventManager = enemyEventManager;

        ToggleSubscription(true);
    }

    private void OnEnable()
    {
        ToggleSubscription(true);
    }

    private void OnDisable()
    {
        ToggleSubscription(false);
    }

    private void OnDestroy()
    {
        ToggleSubscription(false);
    }

    private void ToggleSubscription(bool subscribe)
    {
        if (_enemyEventManager == null) return;

        if (subscribe)
        {
            if (_isSubscribed) return;
            _enemyEventManager.OnStartAttack += OnHit;
            _isSubscribed = true;
        }
        else
        {
            if (!_isSubscribed) return;
            _enemyEventManager.OnStartAttack -= OnHit;
            _isSubscribed = false;
        }
    }

    public override void OnHit(GameObject gameObject)
    {
        if (this == null || gameObject == null || !CanAttack()) return;

        if (gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            var knockbackDirection = (gameObject.transform.position - _enemy.transform.position).normalized;

            KnockbackControoller.Knockback(_enemy.Target, knockbackDirection);

            damageable.TryApplyDamage(_enemy.CalculateTotalDamage());

            _enemyEventManager.TriggerCanceledAttack();
        }
    }

    private bool CanAttack()
    {
        return
            base.CanAttack() &&
            _enemy.CanAttack &&
            _enemy.Target != null &&
            _enemy.Target.transform != null &&
            _enemy.IsDead == false;
    }
}
