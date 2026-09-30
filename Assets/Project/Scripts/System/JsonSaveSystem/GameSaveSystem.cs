using System;
using UnityEngine;
using Zenject;

public class GameSaveSystem : IObjectSaveSystem, IDisposable, ITickable // нужно рефакторить
{
    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;
    private readonly UnitProfileManager _unitProfileManager;
    private readonly LevelProfileManager _levelProfileManager;
    private JsonSaveSystem<GameData> _jsonSaveSystem;

    public GameSaveSystem(Game game, GlobalEventManager globalEventManager, UnitProfileManager unitProfileManager, LevelProfileManager levelProfileManager)
    {
        _game = game;
        _globalEventManager = globalEventManager;
        _unitProfileManager = unitProfileManager;
        _levelProfileManager = levelProfileManager;

        _jsonSaveSystem = new JsonSaveSystem<GameData>();

        LoadObject();
        _globalEventManager.OnExitGame += SaveObject;
    }

    public void LoadObject()
    {
        var gameProfile = _jsonSaveSystem.Load();

        if (gameProfile != null)
        {

            gameProfile.AllPlayers = _game.GameData.AllPlayers;
            gameProfile.AllLevels = _game.GameData.AllLevels;

            //_game.GameData = gameProfile;

            UpdateGameDataFromSerializedProfile(gameProfile);

            UpdateAllLevelsFromSerializedProfile(gameProfile);
        }
        else
        {
            _unitProfileManager.InitializeAllUnitProfile();
            _levelProfileManager.InitializeAllLevelProfile();
        }
    }

    private void UpdateAllLevelsFromSerializedProfile(GameData gameProfile)
    {
        for (int i = 0; i < gameProfile.SerializedAllLevels.Count; i++)
        {
            _game.GameData.AllLevels[i].BaseData.LevelState = gameProfile.SerializedAllLevels[i].LevelState;
        }
    }

    private void UpdateGameDataFromSerializedProfile(GameData gameProfile)
    {
        for (int i = 0; i < gameProfile.AllPlayers.Count; i++)
        {
            gameProfile.AllPlayers[i].UnitPrefab = _game.GameData.AllPlayers[i].UnitPrefab;
            gameProfile.AllPlayers[i].HealthBarPrefab = _game.GameData.AllPlayers[i].HealthBarPrefab;
            gameProfile.AllPlayers[i].SwordChargeBarPrefab = _game.GameData.AllPlayers[i].SwordChargeBarPrefab;
        }

        for (int i = 0; i < gameProfile.SerializedAllPlayers.Count; i++)
        {
            _game.GameData.AllPlayers[i].Data.Health.CurrentValue = gameProfile.SerializedAllPlayers[i].Health.CurrentValue;
            _game.GameData.AllPlayers[i].Data.Health.Price = gameProfile.SerializedAllPlayers[i].Health.Price;
            _game.GameData.AllPlayers[i].Data.Health.NumberDevelop = gameProfile.SerializedAllPlayers[i].Health.NumberDevelop;

            _game.GameData.AllPlayers[i].Data.SpeedMovement.CurrentValue = gameProfile.SerializedAllPlayers[i].SpeedMovement.CurrentValue;
            _game.GameData.AllPlayers[i].Data.SpeedMovement.Price = gameProfile.SerializedAllPlayers[i].SpeedMovement.Price;
            _game.GameData.AllPlayers[i].Data.SpeedMovement.NumberDevelop = gameProfile.SerializedAllPlayers[i].SpeedMovement.NumberDevelop;

            _game.GameData.AllPlayers[i].Data.Damage.CurrentValue = gameProfile.SerializedAllPlayers[i].Damage.CurrentValue;
            _game.GameData.AllPlayers[i].Data.Damage.Price = gameProfile.SerializedAllPlayers[i].Damage.Price;
            _game.GameData.AllPlayers[i].Data.Damage.NumberDevelop = gameProfile.SerializedAllPlayers[i].Damage.NumberDevelop;

        }

        if (gameProfile.SerializedSelectedPlayer != null)
        {
            var savedPlayerData = gameProfile.SerializedSelectedPlayer;

            PlayerProfile selectedPlayer = null;

            foreach (var player in _game.GameData.AllPlayers)
            {
                if (player.Data.Name == savedPlayerData.Name)
                {
                    selectedPlayer = player;
                    break;
                }
            }

            if (selectedPlayer != null)
            {
                _game.GameData.SelectedPlayer.Value = selectedPlayer;

                _game.GameData.SelectedPlayer.Value.Data = gameProfile.SerializedSelectedPlayer;
            }
            else
            {
                if (_game.GameData.AllPlayers.Count > 0)
                {
                    _game.GameData.SelectedPlayer.Value = _game.GameData.AllPlayers[0];
                }
            }
        }
        else
        {
            if (_game.GameData.AllPlayers.Count > 0)
            {
                _game.GameData.SelectedPlayer.Value = _game.GameData.AllPlayers[0];
            }
        }
    }

    public void SaveObject()
    {
        if (_game == null || _game.GameData == null)
        {
            Debug.LogWarning("[SAFE-GUARD] Save skipped: Data container is null during exit.");
            return;
        }

        if (_game.GameData.SelectedPlayer != null && _game.GameData.SelectedPlayer.HasValue)
        {
            var selected = _game.GameData.SelectedPlayer.Value;

            if (selected.Data != null)
            {
                _game.GameData.SerializedSelectedPlayer = selected.Data;
            }
            else
            {
                Debug.LogError("[DEBUG] CRITICAL: selected.Data IS NULL! We are about to assign null.");
            }
        }
        else
        {
            Debug.LogError("[DEBUG] CRITICAL: No player selected at all.");
        }

        _jsonSaveSystem.Save(_game.GameData);
    }

    public void Dispose()
    {
        _globalEventManager.OnStartGame -= LoadObject;
        _globalEventManager.OnExitGame -= SaveObject;
    }

    public void Tick()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveObject();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LoadObject();
            }
        }
    }
}
