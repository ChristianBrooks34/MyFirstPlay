using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnerPlayerButton : IDisposable
{
    private readonly IInstantiator _instantiator;
    private readonly PlayerButton.Factory _playerButtonFactory;
    private readonly GameObject _playerButtonPrefab;
    private readonly Transform _parentForPlayerButton;
    private readonly Game _game;

    private readonly List<PlayerButton> _spawnedButtons = new();
    private bool _isSpawned;

    public SpawnerPlayerButton(GameObject playerButtonPrefab, IInstantiator instantiator, PlayerButton.Factory playerButtonFactory,
        Transform parentForPlayerButton, Game game)
    {
        _game = game;
        _instantiator = instantiator;
        _playerButtonFactory = playerButtonFactory;
        _playerButtonPrefab = playerButtonPrefab;
        _parentForPlayerButton = parentForPlayerButton;
    }

    public void Spawn()
    {
        if (_isSpawned) return;

        int count = Mathf.Min(_game.GameData.AllPlayers.Count, _game.GameData.SerializedAllPlayers.Count);

        for (int i = 0; i < count; i++)
        {
            var go = _instantiator.InstantiatePrefab(_playerButtonPrefab, _parentForPlayerButton);
            var view = go.GetComponent<PlayerButtonView>();

            var playerButton = _playerButtonFactory.Create(view);
            playerButton.Initialize(_game.GameData.AllPlayers[i], _game.GameData.SerializedAllPlayers[i]);

            _spawnedButtons.Add(playerButton);
        }

        _isSpawned = true;
    }

    public void Dispose()
    {
        foreach (var button in _spawnedButtons)
            button.Dispose();

        _spawnedButtons.Clear();
    }
}
