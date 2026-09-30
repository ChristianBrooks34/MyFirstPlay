using System;
using System.Linq;
using UniRx;
using UnityEditor.Rendering;
using UnityEngine;
using Zenject;

public class Game : IDisposable
{
    public GameData GameData;

    private readonly GlobalEventManager _globalEventManager;

    private PlayerProfile _startSelectedPlayer;

    public int ID;

    public Game(GlobalEventManager globalEventManager)
    {
        ID = Time.frameCount;

        Debug.Log($"Game ID = {ID}");

        GameData = new GameData();

        _globalEventManager = globalEventManager;

        GameData.AllPlayers = Resources.LoadAll<PlayerProfile>("Units")
            .ToList();

        GameData.AllLevels = Resources.LoadAll<LevelProfile>("Levels")
            .OrderBy(level => level.BaseData.NumberLevel)
            .ToList();


        GameData.AllLevels.ForEach(level => GameData.SerializedAllLevels.Add(level.BaseData));
        GameData.AllPlayers.ForEach(player => GameData.SerializedAllPlayers.Add(player.Data));

        _startSelectedPlayer = GameData.AllPlayers.FirstOrDefault();

        GameData.SelectedPlayer.Value = _startSelectedPlayer;

        _globalEventManager.OnOpenLevel += ChangeCurrentLevel;
        _globalEventManager.OnExitLevel += OnExitLevel;

        GameData.SelectedPlayer.Subscribe(playerProfile =>
        {
            _globalEventManager.TriggerChangeSelectedPlayer(playerProfile);
        });
    }

    private void ChangeCurrentLevel(LevelData levelData)
    {
        GameData.CurrentLevel = levelData;
    }

    private void OnExitLevel()
    {
        GameData.CurrentLevel = null;
    }

    public void Dispose()
    {
        _globalEventManager.OnOpenLevel -= ChangeCurrentLevel;
        _globalEventManager.OnExitLevel -= OnExitLevel;
    }
}
