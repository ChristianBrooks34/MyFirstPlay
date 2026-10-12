using System;
using UnityEngine;

public class ProjectileTrigerChecker : MonoBehaviour
{
    public event Action<Projectile, GameObject> OnProjectileHit;
    private Projectile _cachedProjectile;

    private void Awake()
    {
        _cachedProjectile = GetComponent<Projectile>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_cachedProjectile != null)
        {
            OnProjectileHit?.Invoke(_cachedProjectile, collision.gameObject);
        }
    }

    private void OnDestroy()
    {
        OnProjectileHit = null;
    }
}
