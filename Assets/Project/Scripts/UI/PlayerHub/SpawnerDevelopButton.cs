using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpawnerDevelopButton : IDisposable
{
    private readonly DiContainer _diContainer;
    private readonly GameObject _developButtonPrefab;
    private readonly Transform _parentForDevelopButton;
    private readonly SoundsEventManager _soundsEventManager;
    private readonly PlayerHub _playerHub;
    private readonly Game _game;

    private List<GameObject> _developButtons = new List<GameObject>();
    private bool _isSpawend;

    public SpawnerDevelopButton(GameObject developButtonPrefab, DiContainer diContainer, Transform parentForDevelopButton,
        Game game, PlayerHub playerHub, SoundsEventManager soundsEventManager)
    {
        _game = game;
        _playerHub = playerHub;
        _diContainer = diContainer;
        _developButtonPrefab = developButtonPrefab;
        _soundsEventManager = soundsEventManager;
        _parentForDevelopButton = parentForDevelopButton;

        _game.GameData.SelectedPlayer.Subscribe(Spawn);

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

        if (_isSpawend)
        {
            foreach (var go in _developButtons)
                GameObject.Destroy(go);
            _developButtons.Clear();
            _isSpawend = false;
        }

        var develops = playerProfile.GetAllDevelops(playerProfile.BaseData);

        if (develops == null) return;

        foreach (var developItem in develops)
        {
            if (developItem == null) continue;

            var go = _diContainer.InstantiatePrefab(_developButtonPrefab, _parentForDevelopButton);
            if (go == null) continue;

            var developButton = _diContainer.Instantiate<DevelopButton>(
                new object[]
                {
                    go.GetComponent<Button>(),
                    _soundsEventManager,
                    go.GetComponent<DisplayDevelopButton>()
                });

            developButton.Initialize(developItem);
            _developButtons.Add(go);
        }

        _isSpawend = true;
    }

    public void Dispose()
    {
        _playerHub.OnOpen -= OnPlayerHubOpen;

        foreach (var go in _developButtons)
            GameObject.Destroy(go);

        _developButtons.Clear();
    }
}
