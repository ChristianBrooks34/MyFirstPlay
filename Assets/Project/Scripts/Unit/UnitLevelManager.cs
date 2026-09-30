using System;
using UnityEngine;

public class UnitLevelManager : IDisposable
{
    private readonly GlobalEventManager _globalEventManager;
    private readonly Game _game;

    public UnitLevelManager(GlobalEventManager globalEventManager, Game game)
    {
        _globalEventManager = globalEventManager;
        _game = game;

        _globalEventManager.OnDeadEnemy += (x) => PlayerLevelUp();
    }

    public void PlayerLevelUp()
    {
        var selectesPlayer = _game.GameData.SelectedPlayer.Value;

        if (selectesPlayer.Data.CurrentLevel >= selectesPlayer.Data.MaxLevel) return;
        if (selectesPlayer.Data.Exp < selectesPlayer.Data.LevelUpExp) return;

        selectesPlayer.Data.Exp -= selectesPlayer.Data.LevelUpExp;

        selectesPlayer.Data.LevelUpExp =
            (int)Mathf.Round(selectesPlayer.Data.LevelUpExp * selectesPlayer.Data.LevelUpExpMultiplayer);

        selectesPlayer.Data.CurrentLevel++;

        selectesPlayer.Data.MaxHealth += (int)selectesPlayer.Data.Health.IncrementValue;

        _globalEventManager.TriggerChangePlayerExp();

        foreach (var developProfile in selectesPlayer.GetAllDevelops(selectesPlayer.Data))
        {
            developProfile.Develop();
        }

        _globalEventManager.TriggerChangePlayerLevel();

        if (selectesPlayer.Data.Exp >= selectesPlayer.Data.LevelUpExp)
        {
            PlayerLevelUp();
        }
    }

    public void EnemyLevelUp(EnemyProfile enemyProfile)
    {
        if (enemyProfile.Data.CurrentLevel >= enemyProfile.Data.MaxLevel)
        {
            return;
        }

        enemyProfile.Data.CurrentLevel++;

        foreach (var developProfile in enemyProfile.GetAllDevelops(enemyProfile.Data))
        {
            developProfile.Develop();
        }
    }

    public void Dispose()
    {
        _globalEventManager.OnDeadEnemy -= (x) => PlayerLevelUp();
    }
}
