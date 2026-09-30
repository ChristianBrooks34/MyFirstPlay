using UnityEngine;
using Zenject;

public class BallController : MonoBehaviour
{
    private Player _player;
    private BallMovement _ballMovement;
    private BallAttack _playerBallAttack;
    private Projectile _projectile;
    private int _direction;

    [Inject]
    public void Construct(BallAttack playerBallAttack, Player player, Projectile projectile)
    {
        _player = player;
        _playerBallAttack = playerBallAttack;

        _projectile = projectile;

        _direction = _player.transform.localScale.x > 0 ? 1 : -1; // ??

        _ballMovement = new BallMovement();
    }

    public void Update()
    {
        if (_ballMovement == null) return;

        _ballMovement.Move(
            _projectile.transform,
            _direction,
            _projectile.Profile.BaseData.Speed);
    }
}
