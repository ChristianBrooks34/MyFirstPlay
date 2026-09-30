using UnityEngine;
using Zenject;

public class SpawnerPlayerButton
{
    private readonly DiContainer _diContainer;
    private readonly GameObject _playerButton;
    private readonly Transform _parentForPlayerButton;
    private readonly Game _game;

    private bool _isSpawend;

    public SpawnerPlayerButton(GameObject playerButton, DiContainer diContainer, Transform parentForPlayerButton,
        Game game)
    {
        _game = game;
        _diContainer = diContainer;
        _playerButton = playerButton;
        _parentForPlayerButton = parentForPlayerButton;
    }

    public void Spawn()
    {
        if (_isSpawend) return;

        for (int i = 0; i < _game.GameData.SerializedAllPlayers.Count; i++)
        {
            var go = _diContainer.InstantiatePrefab(_playerButton, _parentForPlayerButton);

            var playerButton = go.GetComponent<PlayerButton>();

            playerButton.Initialize(_game.GameData.AllPlayers[i],
                _game.GameData.SerializedAllPlayers[i]);
        }
        _isSpawend = true;
    }
}
