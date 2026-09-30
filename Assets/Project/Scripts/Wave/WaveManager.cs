using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

public class WaveManager : IDisposable, IInitializable, ITickable
{
    private readonly LevelController _levelController;
    private readonly EnemyCounter _enemyCounter;
    private readonly LevelEventManager _levelEventManager;
    private readonly EnemyPool _enemyPool;

    private Wave _wave;
    private bool _isPlayerDead;
    private bool _isSubscribedToDeadEnemy;
    private bool _isDisposed; // Защита от двойного Dispose

    private int _id;
    private CancellationTokenSource _cancellationTokenSource; // Для отмены асинхронных задач

    public WaveManager(LevelController levelController, EnemyCounter enemyCounter, EnemyPool enemyPool,
        LevelEventManager levelEventManager)
    {
        _enemyCounter = enemyCounter;
        _levelController = levelController;
        _enemyPool = enemyPool;
        _levelEventManager = levelEventManager;
    }

    public void Initialize()
    {
        _id = Time.frameCount;
        //Debug.LogError($"Создан WaveManager, ID: {_id}");

        _levelEventManager.OnDeadPlayer += OnPlayerDead;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async UniTask StartWave(Wave wave, CancellationToken token = default)
    {
        if (_isDisposed) return;

        //Debug.LogError($"Запуск волны, ID: {_id}");

        // Гарантированно отписываемся от всех предыдущих событий
        UnsubscribeFromEvents();

        // Теперь подписываемся заново
        _isSubscribedToDeadEnemy = true;
        //Debug.Log("+1_isSubscribedToDeadEnemy = " + _isSubscribedToDeadEnemy + $" ID: {_id}");

        _wave = wave;
        _isPlayerDead = false;
        _wave.CurrentAliveEnemies = GetTotalEnemyCount(wave.Enemies);

        _levelEventManager.TriggerStartWave(_wave);

        try
        {
            await SpawnEnemies(token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Волна отменена по токену отмены");
        }
        finally
        {
            _levelEventManager.TriggerCancelWave(_wave);
            UnsubscribeFromEvents(); // Ещё раз убеждаемся, что отписались
            //Debug.Log($"Волна завершена, ID: {_id}");
        }
    }

    private async UniTask SpawnEnemies(CancellationToken token)
    {
        _wave.CurrentAliveEnemies = GetTotalEnemyCount(_wave.Enemies);

        foreach (var enemyInfo in _wave.Enemies)
        {
            for (int i = 0; i < enemyInfo.CountEnemy; i++)
            {
                // Заменяем UniTask.Delay с токеном на цикл с проверкой отмены
                int delay = enemyInfo.SpawnCooldownInMilliseconds;
                while (delay > 0 && !token.IsCancellationRequested)
                {
                    int sleep = Math.Min(delay, 100); // Спим порциями по 100 мс
                    await UniTask.Delay(sleep);
                    delay -= sleep;
                }

                if (token.IsCancellationRequested || _isDisposed || !_levelController.IsLevelOpen)
                    return;

                Debug.Log($"Spawned: {_enemyPool.GetObjectByName(enemyInfo.EnemyProfile.Data.Name).name}");
            }
        }
    }

    private int GetTotalEnemyCount(List<WaveEnemy> waveEnemies)
    {
        return waveEnemies.Sum(x => x.CountEnemy);
    }

    private void OnPlayerDead(Player player)
    {
        //Debug.LogError($"Игрок погиб, ID: {_id}");
        _isPlayerDead = true;
        UnsubscribeFromEvents();
        _cancellationTokenSource.Cancel();
        _levelEventManager.TriggerCancelWave(_wave); // Сразу прерываем волну
    }

    private void UnsubscribeFromEvents()
    {
        //Debug.LogError("+UnsubscribeFromEvents");
        if (_isSubscribedToDeadEnemy)
        {
            _isSubscribedToDeadEnemy = false;
            //Debug.LogError($"----------Отписка от OnDeadEnemy, ID: {_id}");
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        //Debug.LogError($"----------Dispose WaveManager, ID: {_id}");
        _isDisposed = true;

        UnsubscribeFromEvents();
        _levelEventManager.OnDeadPlayer -= OnPlayerDead;
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _wave = null;
    }

    public void Tick()
    {
        //Debug.Log("+_isSubscribedToDeadEnemy = " + _isSubscribedToDeadEnemy + $" ID: {_id}");
    }
}