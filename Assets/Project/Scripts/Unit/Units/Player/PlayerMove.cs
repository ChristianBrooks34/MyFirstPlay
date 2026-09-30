using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMove : BaseMove
{
    private Player _player;
    private PlayerInput _playerInput;
    private PlayerEventManager _playerEventManager;
    private Vector2 _currentOffset;

    [Inject]
    public void Construct(Player player, PlayerInput playerInput, PlayerEventManager playerEventManager)
    {
        _player = player;
        _playerInput = playerInput;
        _playerEventManager = playerEventManager;

        _playerInput.Player.Move.performed += ChangeOffset;
        _playerInput.Player.Move.canceled += StopMove;
    }

    public override void Move()
    {
        if (_player.IsDead) return;
        if (Offset == Vector2.zero) return;

        _player.transform.Translate(Offset * Time.deltaTime);

        _playerEventManager.TriggerMove();
    }

    private void CalculateMoveOffset(float speed, Vector2 input)
    {
        Offset = input * speed;

        ChargeDirection(input);
    }

    private void ChangeOffset(InputAction.CallbackContext context)
    {
        CalculateMoveOffset(
            _player.UnitProfile.BaseData.SpeedMovement.CurrentValue,
            _playerInput.Player.Move.ReadValue<Vector2>());
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        Offset = Vector2.zero;

        ChargeDirection(Offset);
    }
}
