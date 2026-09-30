using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStatsPanel : IDisposable
{
    private readonly SpawnerPlayerStatsItem _spawnerPlayerStatsItem;
    private readonly DisplayPlayerStatsItem _displayPlayerStatsItem;
    private readonly GlobalEventManager _globalEventManager;
    private readonly PlayerHub _playerHub;
    private readonly Game _game;

    private List<PlayerStateItem> _playerStateItems;

    public PlayerStatsPanel(GlobalEventManager globalEventManager, Game game, SpawnerPlayerStatsItem spawnerPlayerStatsItem,
        DisplayPlayerStatsItem displayPlayerStatsItem, PlayerHub playerHub)
    {
        _spawnerPlayerStatsItem = spawnerPlayerStatsItem;
        _displayPlayerStatsItem = displayPlayerStatsItem;
        _globalEventManager = globalEventManager;
        _playerHub = playerHub;
        _game = game;

        _globalEventManager.OnCangeSelectedPlayer += ChangeDisplayPlayerStatsOnCangeSelectedPlayer;

        _globalEventManager.OnOpenPlayerPanel += ChangeDisplayPlayerStatsOnOpenPlayerPanel;
        _globalEventManager.OnDevelopPlayer += ChangeDisplayPlayerStatsOndevelop;

        _playerHub.OnClose += DestroyPlayerStateItems;
    }

    private void ChangeDisplayPlayerStatsOndevelop()
    {
        var develops = _game.GameData.SelectedPlayer.Value.GetAllDevelops(_game.GameData.SelectedPlayer.Value.Data);

        _displayPlayerStatsItem.Display(_playerStateItems, develops);
    }

    private void ChangeDisplayPlayerStatsOnCangeSelectedPlayer(PlayerProfile playerProfile)
    {
        var develops = playerProfile.GetAllDevelops(playerProfile.Data);

        _displayPlayerStatsItem.Display(_playerStateItems, develops);
    }

    private void ChangeDisplayPlayerStatsOnOpenPlayerPanel()
    {
        var develops = _game.GameData.SelectedPlayer.Value.GetAllDevelops(_game.GameData.SelectedPlayer.Value.Data);

        _playerStateItems = _spawnerPlayerStatsItem.Spawn(_game.GameData.SelectedPlayer.Value, develops.Count);

        _displayPlayerStatsItem.Display(_playerStateItems, develops);
    }

    private void DestroyPlayerStateItems()
    {
        foreach (var item in _playerStateItems)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    public void Dispose()
    {
        _globalEventManager.OnCangeSelectedPlayer -= ChangeDisplayPlayerStatsOnCangeSelectedPlayer;

        _globalEventManager.OnOpenPlayerPanel -= ChangeDisplayPlayerStatsOnOpenPlayerPanel;
    }
}
