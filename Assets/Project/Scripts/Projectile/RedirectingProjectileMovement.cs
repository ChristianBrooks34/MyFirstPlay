using UnityEngine;


public class RedirectingProjectileMovement : BaseProjectileMovement
{
    [SerializeField] private Projectile _projectile;

    private Vector3 _direction;

    private void OnEnable()
    {
        if (_projectile != null)
        {
            _projectile.OnInitialized += Initialize;
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        if (_projectile != null)
        {
            _projectile.OnInitialized -= Initialize;
        }
    }

    public override void MoveBall()
    {
        if (_projectile == null || _projectile.gameObject == null) return;
        if (_projectile.Target == null) return;

        _direction = _projectile.Direction.normalized;

        if (!CanMove())
        {
            return;
        }

        var offset = new Vector3(
            _projectile.transform.position.x,
            _projectile.transform.position.y,
            _projectile.transform.position.z) + _direction * _projectile.Profile.BaseData.Speed * Time.deltaTime;

        Rotation.RotateTowardsTarget(_projectile.gameObject, offset, 100f);

        _projectile.transform.position = offset;
    }

    private void Initialize()
    {
        if (_projectile == null) return;

        _direction = _projectile.Direction.normalized;

        float angleInRadians = Mathf.Atan2(_direction.y, _direction.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        _projectile.transform.localRotation = Quaternion.Euler(0, 0, angleInDegrees);
    }

    private bool CanMove()
    {
        if (_projectile == null) return false;

        return _direction != Vector3.zero && !_projectile.IsExplosion;
    }
}


