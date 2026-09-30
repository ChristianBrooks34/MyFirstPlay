using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RedirectingProjectileAttack : BallAttack
{
    private ProjectileContainer _projectileContainer;
    private DiContainer _diContainer;
    private Player _player;

    [Inject]
    public void Construct(DiContainer diContainer, ProjectileContainer projectileContainer, Player player)
    {
        _projectileContainer = projectileContainer;
        _diContainer = diContainer;
        _player = player;
    }

    public override void Attack()
    {
        if (!CanAttack()) return;

        SpawnProjectile(_diContainer, _projectileContainer);

        //AttackLoop(AttackData.Cooldown).Forget();

        TriggerAttack();
    }

    public override void OnHit(GameObject gameObject)
    {
        if (Projectile == null)
        {
            Debug.LogError("Projectile component is missing!");
            return;
        }

        if (!IsLayerInMask(gameObject.layer, layerMask))
        {
            return;
        }

        Projectile.ActiveExplosive(gameObject);
    }

    protected override Vector2 GetProjectileStartDirection()
    {
        return GetMonsterLookDirection();
    }

    protected Vector2 GetMonsterLookDirection()
    {
        if (unit.transform.localScale.x < 0)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }

    protected async UniTaskVoid AttackCooldown()
    {
        while (true)
        {
            await UniTask.Delay(2000);

            if (CanAttack())
            {
                Attack();
            }
        }
    }

    protected override void HandleProjectileHit(Projectile projectile, GameObject gameObject)
    {
        Projectile = projectile;
        OnHit(gameObject);
    }

    protected bool CanAttack()
    {
        return _player != null && !unit.IsDead;
    }
}
