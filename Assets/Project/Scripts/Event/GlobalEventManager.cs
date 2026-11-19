using System;
using UnityEngine;

public class GlobalEventManager
{
    public event Action<LevelData> OnWinLevel;
    public event Action<LevelData> OnOpenLevel;
    public event Action OnExitLevel;
    public event Action OnStartGame;
    public event Action OnExitGame;
    public event Action OnChangePlayerExp;
    public event Action OnChangedPlayerLevel;
    public event Action OnDevelopPlayer;
    public event Action<SceneType> OnChangeScene;
    public event Action OnOpenMainMenu;

    public event Action<int> OnCangeMoneyInWallet;

    public event Action<PlayerProfile> OnCangeSelectedPlayer;

    public event Action OnOpenPlayerPanel;

    public event Action<Enemy> OnDeadEnemy;

    public void TriggerChangeScene(SceneType sceneType)
    {
        Debug.LogError("TriggerChangeScene");
        OnChangeScene?.Invoke(sceneType);
    }

    public void TriggerDeadEnemy(Enemy enemy)
    {
        OnDeadEnemy?.Invoke(enemy);
    }

    public void TriggerStartGame()
    {
        TriggerChangeScene(SceneType.Lobby);
        OnStartGame?.Invoke();
    }

    public void TriggerOpenMainMenu()
    {
        Debug.LogError("TriggerOpenMainMenu");
        TriggerChangeScene(SceneType.MainMenu);
        OnOpenMainMenu?.Invoke();
    }

    public void TriggerExitGame() => OnExitGame?.Invoke();

    public void TriggerCangeMoneyInWallet(int value) => OnCangeMoneyInWallet?.Invoke(value);

    public void TriggerChangeSelectedPlayer(PlayerProfile playerProfile)
    {
        if (playerProfile == null)
        {
            Debug.LogWarning($"{Time.frameCount}***8 GlobalEventManager TriggerChangeSelectedPlayer; playerProfile is NULL! Это нормально при инициализации.");
            return;
        }

        OnCangeSelectedPlayer?.Invoke(playerProfile);
    }

    public void TriggerOpenPlayerPanel() => OnOpenPlayerPanel?.Invoke();

    public void TriggerOpenLevel(LevelData levelProfile)
    {
        TriggerChangeScene(SceneType.Game);
        OnOpenLevel?.Invoke(levelProfile);
    }

    public void TriggerExitLevel()
    {
        TriggerChangeScene(SceneType.Lobby);
        OnExitLevel?.Invoke();
    }
    public void TriggerWinLevel(LevelData levelData)
    {
        OnWinLevel?.Invoke(levelData);
    }

    public void TriggerDevelopPlayer() => OnDevelopPlayer?.Invoke();

    public void TriggerChangePlayerExp() => OnChangePlayerExp?.Invoke();
    public void TriggerChangePlayerLevel() => OnChangedPlayerLevel?.Invoke();
}
