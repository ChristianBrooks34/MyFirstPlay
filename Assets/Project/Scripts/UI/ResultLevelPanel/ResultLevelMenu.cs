using System;
using UnityEngine;


public class ResultLevelMenu : IDisposable
{
    private readonly GameObject _playerLevelMenu;
    private readonly LevelEventManager _levelEventManager;
    private readonly GlobalEventManager _globalEventManager;

    public event Action<bool> OnOpen;
    public event Action OnClouse;
    public event Action OnDisposable;

    public ResultLevelMenu(GameObject playerLevelMenu, LevelEventManager levelEventManager, GlobalEventManager globalEventManager)
    {
        _playerLevelMenu = playerLevelMenu;
        _levelEventManager = levelEventManager;
        _globalEventManager = globalEventManager;

        _globalEventManager.OnWinLevel += OnWinLevel;
        _levelEventManager.OnDeadPlayer += OnDeadPlayer;
    }

    public void Open(bool isWinLevel)
    {
        OnOpen?.Invoke(isWinLevel);
        _playerLevelMenu.SetActive(true);
    }

    public void Clouse()
    {
        OnClouse?.Invoke();
        _playerLevelMenu.SetActive(false);

        Dispose();
    }

    private void OnWinLevel(LevelData levelData)
    {
        Open(true);
    }

    private void OnDeadPlayer(Player player)
    {
        Open(false);
    }

    public void Dispose()
    {
        OnDisposable?.Invoke();

        _globalEventManager.OnWinLevel -= OnWinLevel;
        _levelEventManager.OnDeadPlayer -= OnDeadPlayer;
    }
}

