using System;
using Zenject;

public class PlayerController : ITickable, IDisposable
{
    private Player _player;
    private BaseMove _playerMove;
    private PlayerInput _playerInput;
    private PlayerEventManager _playerEventManager;

    public PlayerController(Player player, BaseMove playerMove, PlayerInput playerInput,
        PlayerEventManager playerEventManager)
    {
        _player = player;
        _playerMove = playerMove;
        _playerInput = playerInput;
        _playerEventManager = playerEventManager;
    }

    public void Tick()
    {
        Move();
    }

    private void Move()
    {
        _playerMove.Move();
    }

    public void Dispose()
    {
        _playerInput?.Disable();
    }
}
