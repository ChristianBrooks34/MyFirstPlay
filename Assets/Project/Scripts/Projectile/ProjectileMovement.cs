using UnityEngine;

public class ProjectileMovement : BaseProjectileMovement
{
    [SerializeField] private Projectile _projectile;

    private void OnEnable()
    {
        _projectile.OnInitialized += Initialize;
    }

    private void OnDisable()
    {
        if (_projectile != null)
        {
            _projectile.OnInitialized -= Initialize;
        }
    }

    public override void MoveBall()
    {
        if (!CanMove()) return;

        _projectile.transform.position += Direction * (_projectile.Profile.BaseData.Speed * Time.deltaTime);
    }

    private void Initialize()
    {
        SetDirection(_projectile.Direction.normalized);

        float angleInRadians = Mathf.Atan2(Direction.y, Direction.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        _projectile.transform.localRotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    private bool CanMove()
    {
        if (_projectile == null) return false;

        return Direction != Vector3.zero && !_projectile.IsExplosion;
    }
}
