using UnityEngine;
using System;
using Zenject;

public class LevelsMenu : IInitializable // нужно рефакторить в 63 строке убрать магическое число
{
    private readonly SpawnerLevelButton _spawnerLevelButton;
    private readonly GlobalEventManager _globalEventManager;
    private readonly Game _game;

    public LevelsMenu(SpawnerLevelButton spawnerLevelButton, Game game, GlobalEventManager globalEventManager)
    {
        _spawnerLevelButton = spawnerLevelButton;
        _globalEventManager = globalEventManager;
        _game = game;

        OnOpenLevelsScene();
    }

    public void Initialize()
    {
        _globalEventManager.OnWinLevel += ChangeLevelsState;
    }

    private void OnOpenLevelsScene()
    {
        foreach (var levelProfile in _game.GameData.AllLevels)
        {
            if (levelProfile.BaseData.NumberLevel == 1 && levelProfile.BaseData.LevelState != LevelState.Completed)
            {
                ChangeLevelsState();
            }

            _spawnerLevelButton.Spawn(levelProfile.BaseData);
        }
    }

    private void ChangeLevelsState(LevelData winLevelData = null)
    {
        if (winLevelData != null) winLevelData.LevelState = LevelState.Completed;

        int numberLastCompletedLevel = 0;

        foreach (var levelProfile in _game.GameData.AllLevels)
        {
            if (levelProfile.BaseData.NumberLevel == 1)
            {
                if (levelProfile.BaseData.LevelState == LevelState.Locked)
                {
                    levelProfile.BaseData.LevelState = LevelState.Available;

                    continue;
                }
            }

            if (levelProfile.BaseData.LevelState == LevelState.Completed)
            {
                numberLastCompletedLevel = levelProfile.BaseData.NumberLevel;
                continue;
            }

            if (numberLastCompletedLevel + 1 == levelProfile.BaseData.NumberLevel)
            {
                levelProfile.BaseData.LevelState = LevelState.Available;
            }
            else
            {
                levelProfile.BaseData.LevelState = LevelState.Locked;
            }
        }
    }
}
