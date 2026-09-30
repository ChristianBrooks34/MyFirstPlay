using UnityEngine;
using Zenject;

public class BallAttack : RangedAttack
{
    [SerializeField] protected GameObject ballPrefab;
    [SerializeField] protected float pushForse;
    [SerializeField] protected float explosionRadius;
    [SerializeField] protected Transform pointSpawnBall;
    [SerializeField] protected Unit unit;
    [SerializeField] protected LayerMask layerMask;

    public override void Attack()
    {
        if (unit.CurrentAttack != null || !CanAttack) return;

        unit.CurrentAttack = this;

        TriggerAttack();
        SpawnProjectile();
    }

    public override void OnHit(GameObject gameObject)
    {
        if (Projectile == null)
        {
            Debug.LogError("Projectile component is missing!");
            return;
        }

        if (Projectile.Profile.BaseData.SourceUnit.gameObject.layer == gameObject.transform.parent.gameObject.layer)
        {
            Debug.LogWarning("Source and target are the same unit — skipping damage.");
            return;
        }

        if (!IsLayerInMask(gameObject.layer, layerMask))
        {
            Debug.LogWarning($"Layer {gameObject.layer} not in mask {layerMask}");
            return;
        }

        Projectile.ActiveExplosive(gameObject);
    }

    public virtual void SpawnProjectile(DiContainer diContainer = null, ProjectileContainer projectileContainer = null)
    {
        if (pointSpawnBall == null || ballPrefab == null || projectileContainer == null) return;

        var ball = InstantiateProjectile(diContainer, projectileContainer);

        var projectile = ball.GetComponent<Projectile>();

        InitializeProjectile(projectile, unit);

        var projectileTrigger = ball.GetComponent<ProjectileTrigerChecker>();

        if (projectileTrigger != null)
        {
            projectileTrigger.OnProjectileHit += HandleProjectileHit;
        }

        projectile.OnExplosion += () => unit.CurrentAttack = null;
    }

    protected virtual GameObject InstantiateProjectile(DiContainer diContainer, ProjectileContainer projectileContainer)
    {
        var ball = diContainer.InstantiatePrefab(
                ballPrefab, pointSpawnBall.position, Quaternion.identity, projectileContainer.transform);

        ball.name = "Projectile " + Time.frameCount;

        return ball;
    }

    protected virtual GameObject InstantiateProjectile(DiContainer diContainer, ProjectileContainer projectileContainer, Vector3 spawnPosition)
    {
        var ball = diContainer.InstantiatePrefab(
                ballPrefab, spawnPosition, Quaternion.identity, projectileContainer.transform);

        ball.name = "Projectile " + Time.frameCount;

        return ball;
    }

    protected virtual void InitializeProjectile(Projectile projectile, Unit target)
    {
        var startDirection = GetProjectileStartDirection();

        projectile.Initialized(startDirection, unit);
        projectile.SetTarget(target);
    }

    protected virtual Vector2 GetProjectileStartDirection()
    {
        return Vector2.zero;
    }

    protected virtual void HandleProjectileHit(Projectile projectile, GameObject gameObject)
    {
        Projectile = projectile;
        OnHit(gameObject);
    }

    protected bool IsLayerInMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
