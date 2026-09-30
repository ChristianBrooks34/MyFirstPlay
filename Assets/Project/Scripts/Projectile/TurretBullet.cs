using Zenject;

public class TurretBullet : Projectile
{
    public override ProjectileContainer ProjectileContainer { get; protected set; }
    public override BaseProjectileMovement ProjectileMovement { get; protected set; }

    [Inject]
    public void Construct(ProjectileContainer projectileContainer)
    {
        ProjectileContainer = projectileContainer;

        ProjectileMovement = GetComponent<ProjectileMovement>();

        ProjectileContainer.Add(ProjectileMovement);

        ProjectileTrigerChecker = GetComponent<ProjectileTrigerChecker>();
        Explosion.OnCanceled += OnExplosionCanceled;

        TimeLiveCooldown().Forget();
    }
}
