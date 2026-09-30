public class RedirectingProjectileEnemyAttack : RedirectingProjectileAttack
{
    private void Start()
    {
        AttackCooldown().Forget();
    }

    protected override void InitializeProjectile(Projectile projectile, Unit target)
    {
        if (target is Enemy)
        {
            var enemy = (Enemy)target;

            var startDirection = GetProjectileStartDirection();

            projectile.Initialized(startDirection, enemy);
            projectile.SetTarget(enemy.Target);
        }
    }
}

