using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : IObjectPool
{
    private readonly SpawnUnitEventManager _spawnUnitEventManager;
    private readonly CursorTileLocator _cursorTileLocator;
    private readonly LevelController _levelController;
    private readonly TileOccupancyMap _tileOccupancyMap;
    private readonly Game _game;
    private Dictionary<string, Queue<Enemy>> _poolByName = new Dictionary<string, Queue<Enemy>>();
    private HashSet<Enemy> _activeObjects = new HashSet<Enemy>();
    private Vector3 _spawnPosition;

    public EnemyPool(LevelController levelController, Game game, SpawnUnitEventManager spawnUnitEventManager,
        CursorTileLocator cursorTileLocator, TileOccupancyMap tileOccupancyMap)
    {
        _game = game;
        _levelController = levelController;
        _tileOccupancyMap = tileOccupancyMap;
        _cursorTileLocator = cursorTileLocator;
        _spawnUnitEventManager = spawnUnitEventManager;

        _spawnUnitEventManager.OnPlayerSpawn += (x) => SpawnGameObjects();
    }

    public GameObject GetObject()
    {
        foreach (var queue in _poolByName.Values)
        {
            if (queue.Count > 0)
            {
                var enemy = queue.Dequeue();
                if (enemy == null || enemy.gameObject == null)
                {
                    Debug.LogWarning("Попытка получить уничтоженный объект!");
                    return null;
                }

                // ЗАМЕНА: используем новый метод с обработкой ошибок
                if (!TryGetValidSpawnPosition(out _spawnPosition, enemy.Target.gameObject.transform.position))
                {
                    Debug.LogError("Не удалось найти валидную позицию для спавна!");
                    ReturnObject(enemy.gameObject);
                    return null;
                }
                enemy.transform.position = _spawnPosition;

                enemy.RecycleActivate();

                Debug.LogError($"EnemyPool GetObject _activeObjects.Add({enemy.name})");

                _activeObjects.Add(enemy);


                _levelController.CountAliveEnemy = _activeObjects.Count;
                return enemy.gameObject;
            }
        }

        Debug.LogWarning("Пул объектов пуст для всех типов!");
        return null;
    }

    public GameObject GetObjectByName(string name)
    {
        if (!_poolByName.ContainsKey(name) || _poolByName[name].Count == 0)
        {
            Debug.LogWarning($"Нет доступных врагов с именем {name} в пуле");
            return null;
        }

        var enemy = _poolByName[name].Dequeue();

        if (enemy == null || enemy.gameObject == null)
        {
            Debug.LogWarning("Попытка получить уничтоженный объект!");
            return null;
        }

        if (enemy is IBuildingEnemy buildingEnemy)
        {
            if (!TryGetValidBuildingSpawnPosition(buildingEnemy, out _spawnPosition, enemy.Target.gameObject.transform.position))
            {
                Debug.LogError($"Не удалось найти позицию для строительного врага: {name}");
                ReturnObject(enemy.gameObject);
                return null;
            }
            enemy.transform.position = _spawnPosition;
        }
        else
        {
            if (!TryGetValidSpawnPosition(out _spawnPosition, enemy.Target.gameObject.transform.position))
            {
                Debug.LogError("Не удалось найти валидную позицию для обычного врага!");
                ReturnObject(enemy.gameObject);
                return null;
            }
            enemy.transform.position = _spawnPosition;
        }

        if (enemy.TriggerChecker != null)
            enemy.TriggerChecker.gameObject.transform.localPosition = Vector3.zero;

        enemy.RecycleActivate();

        _activeObjects.Add(enemy);

        _levelController.CountAliveEnemy = _activeObjects.Count;

        return enemy.gameObject;
    }

    public void ReturnObject(GameObject go)
    {
        if (go == null || go.gameObject == null)
        {
            Debug.LogWarning("Попытка вернуть уничтоженный объект!");
            return;
        }

        var enemy = go.GetComponent<Enemy>();

        enemy.CanAttack = false;

        string enemyName = enemy.UnitProfile.BaseData.Name;

        if (_activeObjects.Contains(enemy))
        {
            enemy.RecycleDeactivate();

            if (!_poolByName.ContainsKey(enemyName))
            {
                _poolByName[enemyName] = new Queue<Enemy>();
            }
            _poolByName[enemyName].Enqueue(enemy);

            _activeObjects.Remove(enemy);
        }
        else
        {
            Debug.LogWarning("Объект не был активен! " + enemyName + "||| IsDead = " + enemy.IsDead);
        }

        _levelController.CountAliveEnemy = _activeObjects.Count;
    }

    private void SpawnGameObjects()
    {
        foreach (var wave in _game.GameData.CurrentLevel.Waves)
        {
            foreach (var waveEnemy in wave.Enemies)
            {
                string enemyName = waveEnemy.EnemyProfile.Data.Name;

                if (!_poolByName.ContainsKey(enemyName))
                {
                    _poolByName[enemyName] = new Queue<Enemy>();
                }

                while (_poolByName[enemyName].Count < waveEnemy.CountEnemy)
                {
                    var enemy = _levelController.SpawnEnemy(waveEnemy.EnemyProfile);

                    if (enemy == null)
                    {
                        Debug.LogError($"Не удалось создать врага с именем {enemyName}");
                        continue;
                    }

                    enemy.SetActive(false);
                    enemy.CanAttack = false;
                    _poolByName[enemyName].Enqueue(enemy);
                }
            }
        }
    }

    public void ClearPool()
    {
        foreach (var queue in _poolByName.Values)
        {
            while (queue.Count > 0)
            {
                var enemy = queue.Dequeue();
                if (enemy.gameObject != null)
                {
                    UnityEngine.Object.Destroy(enemy.gameObject);
                }
            }
        }
        _poolByName.Clear();
        _activeObjects.Clear();
    }

    public int TotalObjectsInPool
    {
        get { return _poolByName.Sum(kvp => kvp.Value.Count); }
    }

    // НОВЫЙ МЕТОД: валидный спавн для обычных врагов
    private bool TryGetValidSpawnPosition(out Vector3 spawnPosition, Vector3 playerPosition)
    {
        const int maxAttempts = 50;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = RandomPointGenerator.GetRandomPointOutsideCameraOnTile(_cursorTileLocator.Tilemap);

            Vector3Int tilePos = _cursorTileLocator.GetTilePosition(randomPoint);

            // ИСПРАВЛЕНИЕ: проверка на занятость — IsOccupied, а не IsCellFree
            if (_cursorTileLocator.HasTileAtWorldPosition(randomPoint) &&
                _tileOccupancyMap.IsCellFree(tilePos))
            {
                spawnPosition = randomPoint;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }

    private bool TryGetValidBuildingSpawnPosition(IBuildingEnemy buildingEnemy, out Vector3 spawnPosition, Vector3 playerPosition)
    {
        const int maxAttempts = 100;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomPoint = RandomPointGenerator.GetRandomPointOnTilemapAdaptive(
                _cursorTileLocator.Tilemap, Camera.main, playerPosition);

            Vector3Int tilePos = _cursorTileLocator.GetTilePosition(randomPoint);

            if (_tileOccupancyMap.IsAreaFree(tilePos, buildingEnemy.Size) &&
                _tileOccupancyMap.OccupyArea(tilePos, buildingEnemy.Size))
            {
                spawnPosition = randomPoint;
                return true;
            }
        }

        // Резервный вариант: пробуем найти любую валидную точку для обычного врага
        if (TryGetValidSpawnPosition(out spawnPosition, playerPosition))
        {
            return true;
        }

        // Дополнительный резервный вариант: поиск свободной области в случайных точках карты
        const int fallbackAttempts = 20;
        for (int fallbackAttempt = 0; fallbackAttempt < fallbackAttempts; fallbackAttempt++)
        {
            // Генерируем случайную точку на всей карте (без адаптивной зоны)
            Vector3 fallbackPoint = RandomPointGenerator.GetRandomPointOnTilemapAdaptive(
                _cursorTileLocator.Tilemap, Camera.main, playerPosition);

            Vector3Int fallbackTilePos = _cursorTileLocator.GetTilePosition(fallbackPoint);

            if (_cursorTileLocator.HasTileAtWorldPosition(fallbackPoint) &&
                _tileOccupancyMap.IsAreaFree(fallbackTilePos, buildingEnemy.Size) &&
                _tileOccupancyMap.OccupyArea(fallbackTilePos, buildingEnemy.Size))
            {
                spawnPosition = fallbackPoint;
                return true;
            }
        }

        // Финальный резервный вариант: используем позицию игрока
        Vector3Int playerTilePos = _cursorTileLocator.GetTilePosition(playerPosition);

        if (_tileOccupancyMap.IsAreaFree(playerTilePos, buildingEnemy.Size) &&
            _tileOccupancyMap.OccupyArea(playerTilePos, buildingEnemy.Size))
        {
            spawnPosition = playerPosition;
            return true;
        }

        // Если все попытки провалились — возвращаем позицию по умолчанию и false
        spawnPosition = Vector3.zero;
        Debug.LogWarning("Не удалось найти валидную позицию для строительного врага после всех попыток!");
        return false;
    }
}