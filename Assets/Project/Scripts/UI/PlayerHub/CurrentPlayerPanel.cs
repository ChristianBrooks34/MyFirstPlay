using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CurrentPlayerPanel : IDisposable
{
    private readonly Text _nameCurrentPlayer;
    private readonly Transform _parentForPlayer;
    private readonly GlobalEventManager _globalEventManager;
    private readonly SpawnUnitForUIFactory _spawnUnitForUIFactory;
    private readonly Game _game;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private GameObject _player;

    public CurrentPlayerPanel(Text nameCurrentPlayer, GlobalEventManager globalEventManager, Game game,
        SpawnUnitForUIFactory spawnUnitForUIFactory, Transform parentForPlayer)
    {
        if (nameCurrentPlayer == null) Debug.LogError("nameCurrentPlayer is null!");
        if (globalEventManager == null) Debug.LogError("globalEventManager is null!");
        if (game == null) Debug.LogError("Game object is null!");
        if (spawnUnitForUIFactory == null) Debug.LogError("SpawnUnitForUIFactory is null!");
        if (parentForPlayer == null) Debug.LogError("parentForPlayer Transform is null!");

        _game = game;
        _parentForPlayer = parentForPlayer;
        _nameCurrentPlayer = nameCurrentPlayer;
        _globalEventManager = globalEventManager;
        _spawnUnitForUIFactory = spawnUnitForUIFactory;

        _globalEventManager.OnCangeSelectedPlayer += ChangeDisplayPlayerOnCangeSelectedPlayer;

        _globalEventManager.OnOpenPlayerPanel += ChangeDisplayPlayerOnOpenPlayerPanel;

        _globalEventManager.OnOpenPlayerPanel += Initialize;
    }

    private void Initialize()
    {
        var current = _game.GameData.SelectedPlayer.Value;


        if (current != null)
        {
            Debug.Log($"{Time.frameCount} [CurrentPlayerPanel] Игрок уже есть в момент создания! Применяем: {current.Data.Name}");
        }
        else
        {
            Debug.LogWarning($"{Time.frameCount} [CurrentPlayerPanel] Игрока нет в момент создания. Ждем события.");
        }
    }

    private void ChangeDisplayPlayerOnOpenPlayerPanel()
    {
        var currentPlayer = _game.GameData.SelectedPlayer.Value;
        if (currentPlayer != null)
        {
            ChangeDisplayPlayer(currentPlayer);
        }
        else
        {
            ClearDisplay();
        }
    }

    private void ChangeDisplayPlayerOnCangeSelectedPlayer(PlayerProfile playerProfile)
    {
        if (playerProfile == null)
        {
            ClearDisplay();
            return;
        }

        ChangeDisplayPlayer(playerProfile);
    }

    public void ChangeDisplayPlayer(PlayerProfile playerProfile)
    {
        if (_parentForPlayer == null)
        {
            Debug.LogError("parentForPlayer Transform is null!");
        }
        if (!_parentForPlayer.gameObject.activeInHierarchy) return;
        if (_nameCurrentPlayer == null || playerProfile == null) return;
     
        _nameCurrentPlayer.text = playerProfile.Data.Name;

        if (_player != null) GameObject.Destroy(_player);

        _player = _spawnUnitForUIFactory.PlayerEmptySpawn(playerProfile, _parentForPlayer, false);
    }

    private void ClearDisplay()
    {
        if (_nameCurrentPlayer != null)
        {
            _nameCurrentPlayer.text = "Нет выбранного игрока"; // Или пустая строка
        }

        //if (_player != null)
        //{
        //    GameObject.Destroy(_player);
        //    _player = null;
        //}
    }

    public void Dispose()
    {
        _disposables.Dispose();

        _globalEventManager.OnCangeSelectedPlayer -= ChangeDisplayPlayerOnCangeSelectedPlayer;
        _globalEventManager.OnOpenPlayerPanel -= ChangeDisplayPlayerOnOpenPlayerPanel;
        _globalEventManager.OnOpenPlayerPanel -= Initialize;

        ClearDisplay();
    }
}
