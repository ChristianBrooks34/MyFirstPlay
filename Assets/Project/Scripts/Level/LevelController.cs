using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    public Transform ParentPlayerHealthBar;
    public Transform ParentPlayer;
    public Transform ParentEnemyHealthBar;
    public Transform ParentEnemy;

    private SpawnUnitEventManager _spawnUnitEventManager;
    private GlobalEventManager _globalEventManager;
    private SpawnUnitFactory _spawnUnitFactory;
    private LevelData _levelData;
    private WaveManager _waveManager;
    private Player _player;
    private Game _game;

    public int CountAliveEnemy;

    public bool IsLevelOpen { get; private set; }

    [Inject]
    public void Construct(SpawnUnitFactory spawnUnitFactory, Game game, WaveManager waveManager,
        SpawnUnitEventManager spawnUnitEventManager, GlobalEventManager globalEventManager)
    {
        _globalEventManager = globalEventManager;
        _spawnUnitEventManager = spawnUnitEventManager;
        _spawnUnitFactory = spawnUnitFactory;
        _waveManager = waveManager;
        _game = game;

        if (_spawnUnitFactory == null)
        {
            Debug.LogError("SpawnUnitFactory not initialized!");
            return;
        }
        _levelData = _game.GameData.CurrentLevel;

        _globalEventManager.OnExitLevel += () => IsLevelOpen = false;

        StartLevel().Forget();
    }

    private async UniTask StartLevel()
    {
        if (_levelData == null) return;

        IsLevelOpen = true;

        await UniTask.Delay(300);
        SpawnPlayer(_game.GameData.SelectedPlayer.Value);

        foreach (var wave in _levelData.Waves)
        {
            await UniTask.Delay(wave.WaveSpawnDelayInMilliseconds);

            wave.IsWaveActive = true;

            if (!IsLevelOpen) break;

            await _waveManager.StartWave(wave);

            while (CountAliveEnemy > 0)
            {
                await UniTask.Delay(100);
            }

            wave.IsWaveActive = false;
        }

        _globalEventManager.TriggerWinLevel(_levelData);
    }

    public Enemy SpawnEnemy(EnemyProfile enemyProfile)
    {
        if (_spawnUnitFactory == null)
        {
            Debug.LogError("SpawnUnitFactory is null in SpawnEnemy!");
            return null;
        }

        if (_player == null)
        {
            Debug.LogWarning("Player is not initialized yet. Cannot spawn enemy.");
            return null;
        }

        var randomPoint = RandomPointGenerator.GetRandomPointOutsideScreenBorder(1);

        var enemy = _spawnUnitFactory.EnemySpawn(
             enemyProfile,
             randomPoint,
             ParentEnemy,
             ParentEnemyHealthBar,
             _player
             );

        return enemy;
    }

    public Player SpawnPlayer(PlayerProfile playerProfile)
    {
        _player = _spawnUnitFactory.PlayerSpawn(
            playerProfile,
            Vector2.zero,
            ParentPlayer,
            ParentPlayerHealthBar
            );

        _spawnUnitEventManager.TriggerPlayerSpawn(_player);

        return _player;
    }
}
