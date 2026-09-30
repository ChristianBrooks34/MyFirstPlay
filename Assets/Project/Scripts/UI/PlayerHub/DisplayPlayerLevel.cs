using System;
using UnityEngine.UI;
using UnityEngine;

public class DisplayPlayerLevel : IDisposable
{
    private readonly Game _game;
    private readonly Text _playerLevel;
    private readonly string _displayPlayerLevel;
    private readonly GlobalEventManager _globalEventManager;
    private readonly PlayerHub _playerHub;

    public DisplayPlayerLevel(Text playerLevel, GlobalEventManager globalEventManager, string displayPlayerLevel,
        PlayerHub playerHub, Game game)
    {
        _game = game;
        _playerHub = playerHub;
        _playerLevel = playerLevel;
        _displayPlayerLevel = displayPlayerLevel;
        _globalEventManager = globalEventManager;

        _playerHub.OnOpen += DisplayOnOpenPlayerHub;
        _globalEventManager.OnCangeSelectedPlayer += Display;

        DisplayOnOpenPlayerHub();
    }

    public void Display(PlayerProfile playerProfile)
    {
        if (_playerLevel.text != null || playerProfile != null)
        {
            _playerLevel.text = string.Format(_displayPlayerLevel, playerProfile.Data.CurrentLevel);
        }
        else
        {
            Debug.LogError("_playerLevel.text == null || playerProfile == null");
        }
    }

    private void DisplayOnOpenPlayerHub()
    {
        Display(_game.GameData.SelectedPlayer.Value);
    }

    public void Dispose() 
    {
        _playerHub.OnOpen -= DisplayOnOpenPlayerHub;
        _globalEventManager.OnCangeSelectedPlayer -= Display;
    }
}
