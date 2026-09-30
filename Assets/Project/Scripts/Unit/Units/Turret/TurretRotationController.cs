using System;
using Zenject;


public class TurretRotationController : ITickable, IDisposable
{
    private readonly EnemyTurretProfile _turretProfile;
    private Turret _turret;
    private bool _isInitialize;

    public TurretRotationController(Turret turret, EnemyTurretProfile turretProfile, Player player)
    {
        _turretProfile = turretProfile;
        _turret = turret;

        if (player != null)
        {
            Initialize(player);
        }
    }

    private void Initialize(Player player)
    {
        _turret.Target = player;
        _isInitialize = true;
    }

    public void Tick()
    {
        if (CanRotate())
        {
            Rotation.RotateTowardsTarget(_turret.CenterRotateHead.gameObject, _turret.Target.transform,
                _turretProfile.RotationSpeed);
        }
    }

    public void Dispose()
    {
        _isInitialize = false;
    }

    private bool CanRotate()
    {
        return _isInitialize && _turret != null;
    }
}
