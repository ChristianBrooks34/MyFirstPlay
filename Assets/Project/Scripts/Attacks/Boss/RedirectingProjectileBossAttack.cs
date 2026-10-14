using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedirectingProjectileBossAttack : RedirectingProjectileAttack
{
    [SerializeField] private float _projectileSpawnIntervalInSeconds;
    [SerializeField] private int _attackProjectileCount;
    [SerializeField] private List<Transform> _spawnPoints;

    private CancellationTokenSource _bossAttackCts;

    private void OnEnable()
    {
        CancelAttackTasks();
        _bossAttackCts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        CancelAttackTasks();
    }

    protected void OnDestroy()
    {
        CancelAttackTasks();
    }

    private void CancelAttackTasks()
    {
        if (_bossAttackCts != null)
        {
            _bossAttackCts.Cancel();
            _bossAttackCts.Dispose();
            _bossAttackCts = null;
        }
    }

    public override void Attack()
    {
        if (_bossAttackCts == null)
        {
            _bossAttackCts = new CancellationTokenSource();
        }

        if (_bossAttackCts.IsCancellationRequested) return;

        ExecuteBurstAttack(_bossAttackCts.Token).Forget();
    }

    private async UniTaskVoid ExecuteBurstAttack(CancellationToken cancellationToken)
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0) return;

        for (int i = 0; i < _attackProjectileCount; i++)
        {
            if (cancellationToken.IsCancellationRequested || this == null || gameObject == null || !gameObject.activeInHierarchy)
                return;

            pointSpawnBall = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            base.Attack();

            bool isCancelled = await UniTask.Delay(
                (int)(_projectileSpawnIntervalInSeconds * 1000),
                cancellationToken: cancellationToken
            ).SuppressCancellationThrow();

            if (isCancelled || this == null || gameObject == null)
                return;
        }
    }

    protected override void InitializeProjectile(Projectile projectile, Unit target)
    {
        if (projectile == null) return;

        if (target is Enemy enemyTarget)
        {
            var startDirection = GetProjectileStartDirection();
            projectile.Initialized(startDirection, enemyTarget);

            if (enemyTarget.Target != null)
            {
                projectile.SetTarget(enemyTarget.Target);
            }
        }
        else
        {
            var startDirection = GetProjectileStartDirection();
            if (unit != null) projectile.Initialized(startDirection, unit);
            if (target != null) projectile.SetTarget(target);
        }
    }

    protected override Vector2 GetProjectileStartDirection()
    {
        if (pointSpawnBall == null || unit == null) return Vector2.right;
        return (pointSpawnBall.position - unit.transform.position).normalized;
    }
}
