using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedirectingProjectileBossAttack : RedirectingProjectileAttack
{
    [SerializeField] private float _projectileSpawnIntervalInSeconds;
    [SerializeField] private int _attackProjectileCount;
    [SerializeField] private List<Transform> _spawnPoints;

    public override void Attack()
    {
        AttackCooldown().Forget();
    }

    public new async UniTaskVoid AttackCooldown()
    {
        for (int i = 0; i < _attackProjectileCount; i++)
        {
            pointSpawnBall = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            base.Attack();

            await UniTask.Delay((int)(_projectileSpawnIntervalInSeconds * 1000));
        }
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

    protected override Vector2 GetProjectileStartDirection()
    {
        return pointSpawnBall.position - unit.transform.position;
    }
}