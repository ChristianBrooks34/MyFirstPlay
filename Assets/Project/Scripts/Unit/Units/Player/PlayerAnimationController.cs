using UnityEngine;
using Zenject;

public class PlayerAnimationController : AnimationController, ITickable
{
    private readonly Player _player;
    private readonly Animator _animator;
    private readonly BaseMove _playerMove;
    private readonly FlipController _spriteController;
    private readonly PlayerEventManager _playerEventManager;


    public PlayerAnimationController(Player player, FlipController spriteController,
        BaseMove playerMove, Animator animator, PlayerEventManager playerEventManager)
    {
        _player = player;
        _animator = animator;
        _playerMove = playerMove;
        _spriteController = spriteController;
        _playerEventManager = playerEventManager;

        _playerEventManager.OnDead += DeadAnimation;
    }

    public void Tick()
    {
        MoveAnimation();
    }

    private void MoveAnimation()
    {
        ControlMoveAnimation(
            _spriteController,
            _animator,
            _playerMove,
            _player.transform);
    }

    private void DeadAnimation()
    {
        ControlDeadAnimation(_animator);
    }
}
