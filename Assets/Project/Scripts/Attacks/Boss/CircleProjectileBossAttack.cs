using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CircleProjectileBossAttack : BallAttack
{
    [SerializeField] private int CountSpawnProjectile;
    [SerializeField] private bool _isRandomSpawn;

    private ProjectileContainer _projectileContainer;
    private DiContainer _diContainer;
    private Player _player;
    private Vector3 spawnPosition;
    private Vector3 _centerCircle;
    private Unit _target;

    [Inject]
    public void Construct(DiContainer diContainer, ProjectileContainer projectileContainer, Player player)
    {
        _projectileContainer = projectileContainer;
        _diContainer = diContainer;
        _player = player;
    }

    public override void Attack()
    {
        if (!CanAttack || _projectileContainer == null || unit == null || !unit.CanAttack) return;
        if (_player == null || _player.IsDead) return;

        _centerCircle = unit.transform.position;
        unit.CurrentAttack = this;

        TriggerAttack();

        List<Vector3> positions = new List<Vector3>();

        if (_isRandomSpawn)
        {
            positions = RandomPointGenerator.GetRandomPointOnCircle(CountSpawnProjectile, 1f, _centerCircle);
        }
        else
        {
            positions = PositionGenerator.GetPointInCircle(CountSpawnProjectile, 1f, _centerCircle);
        }

        for (var i = 0; i < positions.Count; i++)
        {
            SpawnProjectileAtPosition(_diContainer, _projectileContainer, positions[i]);
        }
    }

    private void SpawnProjectileAtPosition(DiContainer diContainer, ProjectileContainer projectileContainer, Vector3 position)
    {
        if (ballPrefab == null) return;

        var ball = InstantiateProjectile(diContainer, projectileContainer, position);
        var projectile = ball.GetComponent<Projectile>();

        InitializeProjectile(projectile, _player);

        var projectileTrigger = ball.GetComponent<ProjectileTrigerChecker>();
        if (projectileTrigger != null)
        {
            projectileTrigger.OnProjectileHit += HandleProjectileHit;
        }

        projectile.OnExplosion += ResetUnitAttack;
    }

    private void ResetUnitAttack()
    {
        if (unit != null) unit.CurrentAttack = null;
    }

    protected override GameObject InstantiateProjectile(DiContainer diContainer, ProjectileContainer projectileContainer)
    {
        return InstantiateProjectile(diContainer, _projectileContainer, spawnPosition);
    }

    protected override void InitializeProjectile(Projectile projectile, Unit target)
    {
        var startDirection = unit.transform.position - projectile.transform.position;

        _target = target;

        projectile.Initialized(startDirection, unit);
        projectile.SetTarget(target);
    }
}

