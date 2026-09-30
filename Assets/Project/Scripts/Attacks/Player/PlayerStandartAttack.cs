using UnityEngine;
using Zenject;

public class PlayerStandardAttack : BaseAttack
{
    private Player _player;
    private LayerMask _layerMask;
    private WeaponUpDownAnimationController _swordAnimationController;

    [Inject]
    public void Construct(Player player, WeaponUpDownAnimationController swordAnimationController)
    {
        _layerMask = LayerMask.NameToLayer("Enemy");
        _player = player;
        _swordAnimationController = swordAnimationController;

        _player.CurrentAttack = this;

        _swordAnimationController.Attack += Attack;
        _swordAnimationController.OnStartedSwordUpAnimation += () => _player.CurrentAttack = this;
    }

    public override void Attack()
    {
        if (!CanAttack) return;
        if (_player.CurrentAttack != this) return;

        TriggerAttack();

        var colliders = AttackHitChecker.GetFrontTargetsInAttackRange(
            _player.transform, _layerMask, AttackData.AttackRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                OnHit(collider);
            }
        }

        //AttackLoop(AttackData.Cooldown).Forget();
    }

    public override void OnHit(GameObject gameObject)
    {
        var enemy = gameObject.GetComponent<Enemy>();

        enemy.ApplyDamage(_player.CalculateTotalDamage());

        KnockbackControoller.Knockback
        (
            enemy,
            enemy.transform.position - _player.transform.position
        );
    }
}
