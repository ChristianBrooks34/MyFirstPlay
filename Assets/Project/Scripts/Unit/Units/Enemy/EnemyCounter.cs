using System;
using UnityEngine;
using Zenject;


public class EnemyCounter : IInitializable, IDisposable
{
    private readonly GlobalEventManager _globalEventManager;
    public int CountAliveEnemy { get; private set; }

    public EnemyCounter(GlobalEventManager globalEventManager)
    {
        _globalEventManager = globalEventManager;
    }

    public void Initialize()
    {
        _globalEventManager.OnDeadEnemy += OnDeadEnemy;
    }

    public void OnDeadEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        CountAliveEnemy -= 1;
    }

    public void OnSpawnEnemy()
    {
        CountAliveEnemy += 1;
    }

    public void Dispose()
    {
        CountAliveEnemy = 0;
        _globalEventManager.OnDeadEnemy -= OnDeadEnemy;
    }
}
