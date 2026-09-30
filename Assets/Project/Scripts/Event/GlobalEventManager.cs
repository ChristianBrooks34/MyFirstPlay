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

    public event Action<int> OnCangeMoneyInWallet;

    public event Action<PlayerProfile> OnCangeSelectedPlayer;

    public event Action OnOpenPlayerPanel;

    public event Action<Enemy> OnDeadEnemy;

    private int ID;

    public GlobalEventManager()
    {
        ID = Time.frameCount;
    }

    public int GetInstanceID()
    {
        return ID;
    }

    public void TriggerDeadEnemy(Enemy enemy)
    {
        OnDeadEnemy?.Invoke(enemy);
    }

    public void TriggerStartGame() => OnStartGame?.Invoke();
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

    public void TriggerOpenLevel(LevelData levelProfile) => OnOpenLevel?.Invoke(levelProfile);
    public void TriggerExitLevel() => OnExitLevel?.Invoke();

    public void TriggerWinLevel(LevelData levelData)
    {
        OnWinLevel?.Invoke(levelData);
    }

    public void TriggerDevelopPlayer() => OnDevelopPlayer?.Invoke();

    public void TriggerChangePlayerExp() => OnChangePlayerExp?.Invoke();
    public void TriggerChangePlayerLevel() => OnChangedPlayerLevel?.Invoke();
}
