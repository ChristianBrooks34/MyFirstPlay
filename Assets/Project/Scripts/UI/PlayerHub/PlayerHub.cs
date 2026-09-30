using System;
using UnityEngine;

public class PlayerHub
{
    private readonly GameObject _playerHub;
    private readonly SpawnerPlayerButton _spawnerPlayerButton;

    public event Action OnOpen;
    public event Action OnClose;

    public PlayerHub(GameObject playerHub, SpawnerPlayerButton spawnerPlayerButton)
    {
        _playerHub = playerHub;
        _spawnerPlayerButton = spawnerPlayerButton;
    }

    public void Open()
    {
        _playerHub.SetActive(true);
        OnOpen?.Invoke();

        _spawnerPlayerButton.Spawn();
    }

    public void Clouse()
    {
        _playerHub.SetActive(false);
        OnClose?.Invoke();
    }
}
