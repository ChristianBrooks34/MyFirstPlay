using System;
using UnityEngine;
using Zenject;


public class LevelEventManager : IInitializable, IDisposable
{
    public event Action OnDeadEnemy;
    public event Action<Player> OnDeadPlayer;
    public event Action<Wave> OnStartWave;
    public event Action<Wave> OnCancelWave;

    private readonly GlobalEventManager _globalEventManager;

    public LevelEventManager(GlobalEventManager globalEventManager)
    {
        _globalEventManager = globalEventManager;
    }

    public void TriggerDeadPlayer(Player player) => OnDeadPlayer?.Invoke(player);

    public void TriggerStartWave(Wave wave) => OnStartWave?.Invoke(wave);
    public void TriggerCancelWave(Wave wave) => OnCancelWave?.Invoke(wave);

    private void OnDeadEnemyHandler(Enemy enemy)
    {
        OnDeadEnemy?.Invoke();
    }

    public void Initialize()
    {
        _globalEventManager.OnDeadEnemy += OnDeadEnemyHandler;
    }

    public void Dispose()
    {
        _globalEventManager.OnDeadEnemy -= OnDeadEnemyHandler;
        OnDeadEnemy = null;
    }
}

