using Zenject;

public class RedirectingBullet : Projectile
{
    public override ProjectileContainer ProjectileContainer { get; protected set; }
    public override BaseProjectileMovement ProjectileMovement { get; protected set; }

    [Inject]
    public void Construct(ProjectileContainer projectileContainer)
    {
        ProjectileContainer = projectileContainer;

        ProjectileMovement = GetComponent<RedirectingProjectileMovement>();

        ProjectileContainer.Add(ProjectileMovement);

        ProjectileTrigerChecker = GetComponent<ProjectileTrigerChecker>();
        Explosion.OnCanceled += OnExplosionCanceled;

        TimeLiveCooldown().Forget();
    }
}
