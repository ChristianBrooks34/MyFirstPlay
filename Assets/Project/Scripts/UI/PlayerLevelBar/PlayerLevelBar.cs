using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerLevelBar : ValueBar, IInitializable, IDisposable
{
    private DisplayPlayerLevelBar _displayValueBar;
    private Text _displayText;
    private GlobalEventManager _globalEventManager;
    private UnitLevelManager _unitLevelManager;
    private Game _game;

    [Inject]
    public void Construct(GlobalEventManager globalEventManager, Game game, UnitLevelManager unitLevelManager,
        DisplayPlayerLevelBar displayPlayerLevelBar, Text displayText)
    {
        _globalEventManager = globalEventManager;
        _unitLevelManager = unitLevelManager;
        _displayValueBar = displayPlayerLevelBar;
        _displayText = displayText;
        _game = game;

        _displayValueBar.TextCount = _displayText;

        UpdatePlayerLevelBar();
    }

    public void Initialize()
    {
        _globalEventManager.OnDeadEnemy += OnDeadEnemy;

        _globalEventManager.OnChangedPlayerLevel += UpdatePlayerLevelBar;

        UpdatePlayerLevelBar();
    }

    private void UpdatePlayerLevelBar()
    {
        _displayValueBar.Display(_game.GameData.SelectedPlayer.Value.Data.Exp,
            _game.GameData.SelectedPlayer.Value.Data.LevelUpExp, 0.5f, true);
    }

    private void OnDeadEnemy(Enemy enemy)
    {
        if (enemy == null || enemy.UnitProfile == null) return;

        if (_game == null)
        {
            Debug.LogWarning("Game instance is null during enemy death event.");
            return;
        }

        _game.GameData.SelectedPlayer.Value.Data.Exp += enemy.UnitProfile.BaseData.Exp;

        // Проверка менеджера уровней
        if (_unitLevelManager != null)
        {
            _unitLevelManager.PlayerLevelUp();
        }

        StartCoroutine(DelayedDisplayUpdate());
    }

    IEnumerator DelayedDisplayUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        UpdatePlayerLevelBar();
    }

    private void Dispose()
    {
        _globalEventManager.OnDeadEnemy -= OnDeadEnemy;

        _globalEventManager.OnChangedPlayerLevel -= UpdatePlayerLevelBar;
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }
}
