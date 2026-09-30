using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class SpawnerDevelopButton : IDisposable
{
    private readonly DiContainer _diContainer;
    private readonly GameObject _developButtonPrefab;
    private readonly Transform _parentForDevelopButton;
    private readonly PlayerHub _playerHub;
    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;

    private List<GameObject> _developButtons = new List<GameObject>();
    private bool _isSpawend;

    public SpawnerDevelopButton(GameObject developButtonPrefab, DiContainer diContainer, Transform parentForDevelopButton,
        Game game, PlayerHub playerHub, GlobalEventManager globalEventManager)
    {
        _game = game;
        _playerHub = playerHub;
        _diContainer = diContainer;
        _globalEventManager = globalEventManager;
        _developButtonPrefab = developButtonPrefab;
        _parentForDevelopButton = parentForDevelopButton;

        // Добавляем подписку прямо в контейнер
        _game.GameData.SelectedPlayer.Subscribe(Spawn);

        // Аналогично для событий (если нужно хранить подписку явно)
        //_playerHub.OnOpen += OnPlayerHubOpen;
        _globalEventManager.OnOpenPlayerPanel += OnPlayerHubOpen;
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

            var developButton = go.GetComponent<DevelopButton>();
            if (developButton == null)
            {
                GameObject.Destroy(go);
                continue;
            }

            developButton.Initialize(developItem);
            _developButtons.Add(go);
        }

        _isSpawend = true;
    }

    public void Dispose()
    {
        // 2. Отписываемся от событий (если использовали += без контейнера)
        _playerHub.OnOpen -= OnPlayerHubOpen;

        // 3. Чистим UI
        foreach (var go in _developButtons)
            GameObject.Destroy(go);
        _developButtons.Clear();
    }
}
