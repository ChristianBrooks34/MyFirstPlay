using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BulletTurretAttack : BallAttack
{
    private DiContainer _diContainer;
    private Turret _turret;

    [Inject]
    public void Construct(Turret turret, DiContainer diContainer)
    {
        _diContainer = diContainer;
        _turret = turret;

        AttackCooldown().Forget();
    }

    public override void Attack()
    {
        if (!base.CanAttack) return;

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

        //if (!IsLayerInMask(_gameObject.layer, layerMask))
        //{
        //    Debug.LogWarning($"Layer {_gameObject.layer} not in mask {layerMask}");
        //    return;
        //}

        Projectile.ActiveExplosive(gameObject);
    }

    public override void SpawnProjectile(DiContainer diContainer = null, ProjectileContainer projectileContainer = null)
    {
        if (pointSpawnBall == null || gameObject == null) return;

        var ball = _diContainer.InstantiatePrefab(ballPrefab, pointSpawnBall.position, Quaternion.identity, null);

        ball.name = "Projectile " + Time.frameCount;

        ball.transform.rotation = _turret.CenterRotateHead.rotation;

        var projectile = ball.GetComponent<Projectile>();

        var direction = _turret.PointBulletSpawn.position - _turret.CenterRotateHead.position;

        projectile.Initialized(direction, _turret);

        var projectileTrigger = ball.GetComponent<ProjectileTrigerChecker>();

        if (projectileTrigger != null)
        {
            projectileTrigger.OnProjectileHit += HandleProjectileHit;
        }

        projectile.OnExplosion += () => unit.CurrentAttack = null;

        //AttackLoop(AttackData.Cooldown).Forget();
    }

    private async UniTaskVoid AttackCooldown()
    {
        await UniTask.Delay(Random.Range(0, 1000));

        while (true)
        {
            await UniTask.Delay(2000);

            if (CanAttack())
            {
                Attack();
            }
        }
    }

    private bool CanAttack()
    {
        return _turret.Target != null && !unit.IsDead && unit.CanAttack;
    }
}
