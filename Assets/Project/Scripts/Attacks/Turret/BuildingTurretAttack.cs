using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class BuildingTurretAttack : BallAttack // нужен рефакторинг
{
    [SerializeField] private Transform _pointBulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    private BulletsContainer _bulletsContainer;
    private DiContainer _diContainer;
    private TargetLocator _targetLocator;
    private TurretBuilding _turretBuilding;

    private CancellationTokenSource _cancellationTokenSource;
    private BuildingEventManager _buildingEventManager;

    private bool _isDestroyed = false;

    [Inject]
    public void Construct(TargetLocator targetLocator, DiContainer diContainer, BulletsContainer bulletsContainer,
        BuildingEventManager buildingEventManager)
    {
        _diContainer = diContainer;
        _targetLocator = targetLocator;
        _bulletsContainer = bulletsContainer;
        _buildingEventManager = buildingEventManager;

        _turretBuilding = GetComponent<TurretBuilding>();

        if (_turretBuilding == null) Debug.LogError("_turretBuilding == null");

        _buildingEventManager.OnInitialize += StartAttack;
    }

    private void StartAttack()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            return;

        _cancellationTokenSource = new CancellationTokenSource();
        MainCombatLoop(_cancellationTokenSource.Token).Forget();
    }

    public override void Attack()
    {
        if (_isDestroyed || !gameObject.activeInHierarchy || _turretBuilding == null)
            return;

        if (_turretBuilding.Target != null && !_turretBuilding.Target.IsDead)
        {
            SpawnBullet();
        }
        else
        {
            _turretBuilding.Target = null;
        }
    }

    private async UniTask MainCombatLoop(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested && !_isDestroyed)
            {
                if (!IsObjectValid())
                    break;

                if (_turretBuilding.CurrentState != BuildingState.Completed)
                {
                    try
                    {
                        await UniTask.DelayFrame(10, cancellationToken: cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    continue;
                }

                _turretBuilding.Target = _targetLocator.GetNearestEnemy(transform, _turretBuilding.TurretProfile.Data.RadiusAttack);

                if (_turretBuilding.Target == null)
                {
                    try
                    {
                        await WaitSearchDelay(cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    continue;
                }

                await AttackTargetLoop(cancellationToken);

                if (IsObjectValid())
                    _turretBuilding.Target = null;
            }
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
            if (!_isDestroyed)
            {
                Debug.LogError($"Ошибка в боевом цикле: {ex.Message}");
            }
        }
    }

    private async UniTask WaitSearchDelay(CancellationToken cancellationToken)
    {
        await UniTask.Delay((int)(_turretBuilding.TurretProfile.Data.AttackCooldownInSeconds * 1000 / 2),
            cancellationToken: cancellationToken);
    }

    private async UniTask AttackTargetLoop(CancellationToken cancellationToken)
    {
        while (_turretBuilding.Target != null && !_turretBuilding.Target.IsDead &&
            !cancellationToken.IsCancellationRequested && !_isDestroyed)
        {
            if (!IsObjectValid())
                break;

            try
            {
                await WaitAttackCooldown(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (IsObjectValid())
            {
                Attack();
            }
        }
    }

    private async UniTask WaitAttackCooldown(CancellationToken cancellationToken)
    {
        await UniTask.Delay((int)(_turretBuilding.TurretProfile.Data.AttackCooldownInSeconds * 1000),
            cancellationToken: cancellationToken);
    }

    private bool IsObjectValid()
    {
        return !_isDestroyed &&
               _turretBuilding != null &&
               gameObject != null &&
               gameObject.activeInHierarchy &&
               this != null;
    }

    private void SpawnBullet()
    {
        if (_isDestroyed) return;

        if (_pointBulletSpawn == null || _pointBulletSpawn.gameObject == null) return;
        if (_bulletPrefab == null) return;
        if (_bulletsContainer == null || _bulletsContainer.transform == null) return;

        var ball = _diContainer.InstantiatePrefab(
            _bulletPrefab,
            _pointBulletSpawn.position,
            Quaternion.identity,
            _bulletsContainer.transform
        );

        if (ball == null)
        {
            Debug.LogError("Не удалось создать снаряд!");
            return;
        }

        ball.name = "Projectile " + Time.frameCount;
        var projectile = ball.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.LogError("На снаряде отсутствует компонент Projectile!");
            return;
        }

        if (_turretBuilding?.Target == null || _isDestroyed)
            return;

        var direction = _turretBuilding.Target.transform.position - transform.position;
        projectile.Initialized(direction, _turretBuilding.SourceUnit);

        var projectileTrigger = ball.GetComponent<ProjectileTrigerChecker>();

        if (projectileTrigger != null)
        {
            projectileTrigger.OnProjectileHit += HandleProjectileHit;
        }
    }

    protected override void HandleProjectileHit(Projectile projectile, GameObject gameObject)
    {
        if (_isDestroyed) return;
        Debug.LogError("HandleProjectileHit");

        projectile.ActiveExplosive(gameObject);
    }

    private void OnDestroy()
    {
        _isDestroyed = true;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _buildingEventManager.OnInitialize -= StartAttack;
    }
}
