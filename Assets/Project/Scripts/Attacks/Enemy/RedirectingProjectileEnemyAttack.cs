using Zenject;

public class RedirectingProjectileEnemyAttack : RedirectingProjectileAttack
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

    private void Start()
    {
        AttackCooldown().Forget();
    }

    protected override void InitializeProjectile(Projectile projectile, Unit target)
    {
        if (projectile == null) return;

        if (target is Enemy)
        {
            var enemy = (Enemy)target;

            var startDirection = GetProjectileStartDirection();

            projectile.Initialized(startDirection, enemy);
            projectile.SetTarget(enemy.Target);
        }
        else
        {
            var startDirection = GetProjectileStartDirection();
            if (unit != null) projectile.Initialized(startDirection, unit);
            if (target != null) projectile.SetTarget(target);
        }
    }
}

