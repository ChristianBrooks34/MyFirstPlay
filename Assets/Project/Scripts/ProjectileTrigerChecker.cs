using System;
using UnityEngine;

public class ProjectileTrigerChecker : MonoBehaviour
{
    public event Action<Projectile, GameObject> OnProjectileHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.GetComponent<Projectile>() != null)
        {
            if (OnProjectileHit != null)
            {
                OnProjectileHit?.Invoke(gameObject.GetComponent<Projectile>(), collision.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        OnProjectileHit = null;
    }
}
