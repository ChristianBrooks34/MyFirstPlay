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
        if (!CanAttack) return;
        if (_projectileContainer == null) return;
        if (!unit.CanAttack) return;
        if (_target.IsDead) return;

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
            spawnPosition = positions[i];
            SpawnProjectile(_diContainer, _projectileContainer);
        }
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

