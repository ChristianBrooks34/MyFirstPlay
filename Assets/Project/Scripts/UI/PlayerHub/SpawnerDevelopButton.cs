using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpawnerDevelopButton : IDisposable
{
    private readonly DevelopButton.Factory _developButtonFactory;
    private readonly GameObject _developButtonPrefab;
    private readonly Transform _parentForDevelopButton;
    private readonly PlayerHub _playerHub;
    private readonly Game _game;
    private readonly List<(DevelopButton controller, GameObject view)> _spawnedButtons = new();
    private readonly IDisposable _playerSubscription;
    private readonly IInstantiator _instantiator;

    public SpawnerDevelopButton(GameObject developButtonPrefab, DevelopButton.Factory developButtonFactory,
        Transform parentForDevelopButton, Game game, PlayerHub playerHub, IInstantiator instantiator)
    {
        _game = game;
        _playerHub = playerHub;
        _instantiator = instantiator;
        _developButtonFactory = developButtonFactory;
        _developButtonPrefab = developButtonPrefab;
        _parentForDevelopButton = parentForDevelopButton;

        _playerSubscription = _game.GameData.SelectedPlayer.Subscribe(Spawn);
        _playerHub.OnOpen += OnPlayerHubOpen;
    }

    private void OnPlayerHubOpen()
    {
        var currentPlayer = _game.GameData.SelectedPlayer.Value;
        if (currentPlayer != null)
        {
            Spawn(currentPlayer);
        }
    }

    private void Spawn(PlayerProfile playerProfile)
    {
        if (playerProfile == null) return;

        ClearButtons();

        var develops = playerProfile.GetAllDevelops(playerProfile.BaseData);
        if (develops == null) return;

        foreach (var developItem in develops)
        {
            if (developItem == null) continue;

            var go = _instantiator.InstantiatePrefab(_developButtonPrefab, _parentForDevelopButton);
            if (go == null) continue;

            var developButton = _developButtonFactory.Create(
                go.GetComponent<Button>(),
                go.GetComponent<DisplayDevelopButton>());

            developButton.Initialize(developItem);
            _spawnedButtons.Add((developButton, go));
        }
    }

    private void ClearButtons()
    {
        foreach (var (controller, view) in _spawnedButtons)
        {
            controller.Dispose();
            if (view != null)
                GameObject.Destroy(view);
        }
        _spawnedButtons.Clear();
    }

    public void Dispose()
    {
        _playerSubscription?.Dispose();
        _playerHub.OnOpen -= OnPlayerHubOpen;
        ClearButtons();
    }
}
