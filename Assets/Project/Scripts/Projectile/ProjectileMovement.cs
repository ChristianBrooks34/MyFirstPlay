using UnityEngine;

public class ProjectileMovement : BaseProjectileMovement
{
    [SerializeField] private Projectile _projectile;

    private bool _isDisable = false;

    private void OnEnable()
    {
        _projectile.OnInitialized += Initialize;
    }

    private void OnDisable()
    {
        _isDisable = true;
        _projectile.OnInitialized -= Initialize;
    }

    public override void MoveBall()
    {
        if (!CanMove()) return;

        var offset = new Vector3(
            _projectile.transform.position.x,
            _projectile.transform.position.y,
            _projectile.transform.position.z) + Direction * _projectile.Profile.BaseData.Speed * Time.deltaTime;

        _projectile.transform.position = offset;
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
