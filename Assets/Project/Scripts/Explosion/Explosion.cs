using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Explosion : MonoBehaviour // редактировал
{
    private Animator _animator;
    private DamageRules _damageRules;

    public event Action OnCanceled;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    [Inject]
    public void Construct(DamageRules damageRules)
    {
        _damageRules = damageRules;
    }

    public void Explosive(Projectile projectile, GameObject directHitGameObject = null)
    {
        if (projectile == null || projectile.Profile == null || projectile.Profile.BaseData.SourceUnit == null)
            return;

        var sourceUnit = projectile.Profile.BaseData.SourceUnit;
        var attackerType = sourceUnit.Type;

        if (_damageRules == null)
        {
            Debug.LogError("[Explosion] DamageRules not assigned in GameConfig!");
            return;
        }

        var allTargetsColliders = GetAllTargetsColliders(projectile);

        List<GameObject> targets = GetTargetList(allTargetsColliders, projectile, directHitGameObject);

        StartAnimation();

        ApplyDirectHitDamage(projectile, directHitGameObject);

        if (targets.Count == 0)
        {
            Debug.LogWarning("[Explosion] No valid targets after filtering.");
            return;
        }

        ApplyExplosiveDamage(projectile, targets);
    }

    private void ApplyExplosiveDamage(Projectile projectile, List<GameObject> targets)
    {
        float explosiveRange = projectile.Profile.BaseData.ExplosiveRange;
        float explosiveRangeSqr = explosiveRange * explosiveRange;

        foreach (var targetGo in targets)
        {
            Vector3 toTarget = targetGo.transform.position - projectile.transform.position;
            float sqrDistance = toTarget.sqrMagnitude;

            if (sqrDistance > explosiveRangeSqr)
                continue;

            IDamageable damageable = targetGo.GetComponent<IDamageable>();
            IKnockbackable knockbackable = targetGo.GetComponent<IKnockbackable>();

            if (damageable == null || knockbackable == null)
                continue;

            float distance = Mathf.Sqrt(sqrDistance);
            float coefficient = Mathf.Clamp01((explosiveRange - distance) / explosiveRange);
            float finalDamage = projectile.Profile.BaseData.MinDamage +
                (projectile.Profile.BaseData.MaxDamage - projectile.Profile.BaseData.MinDamage) * coefficient;

            float totalDamage = projectile.Profile.BaseData.SourceUnit.CalculateTotalDamage() + finalDamage;
            damageable.TryApplyDamage(totalDamage);

            if (toTarget.sqrMagnitude > 0.001f)
                knockbackable.Knockback(toTarget.normalized);
        }
    }

    private void ApplyDirectHitDamage(Projectile projectile, GameObject directHitGameObject = null)
    {
        if (directHitGameObject != null)
        {
            var directUnit = directHitGameObject.GetComponent<ITargetInfo>();

            if (directUnit != null && _damageRules.CanDamage(projectile.Profile.BaseData.SourceUnit.Type, directUnit.Type))
            {
                IDamageable damageable = directHitGameObject.GetComponent<IDamageable>();
                IKnockbackable knockbackable = directHitGameObject.GetComponent<IKnockbackable>();

                if (damageable != null)
                {
                    float baseDamage = projectile.Profile.BaseData.SourceUnit.CalculateTotalDamage();
                    damageable.TryApplyDamage(baseDamage + projectile.Profile.BaseData.MaxDamage);
                }

                if (knockbackable != null)
                {
                    Vector3 toCollider = directHitGameObject.transform.position - projectile.transform.position;
                    if (toCollider.sqrMagnitude > 0.001f)
                        knockbackable.Knockback(toCollider.normalized);
                }
            }
        }
    }

    private List<GameObject> GetTargetList(List<GameObject> allTargetsColliders, Projectile projectile, GameObject directHitGameObject = null)
    {
        List<GameObject> targets = new List<GameObject>();

        foreach (var collider in allTargetsColliders)
        {
            var targetGo = collider.transform.parent.gameObject;
            if (targetGo == null) continue;

            var unit = targetGo.GetComponent<ITargetInfo>();
            if (unit == null || unit.Type == default)
                continue;

            if (_damageRules.CanDamage(projectile.Profile.BaseData.SourceUnit.Type, unit.Type))
                continue;

            if (!targets.Contains(targetGo))
            {
                if (directHitGameObject != null && directHitGameObject != targetGo)
                    continue;

                targets.Add(targetGo);
            }
        }

        return targets;
    }

    private List<GameObject> GetAllTargetsColliders(Projectile projectile)
    {
        return AttackHitChecker.GetAllTargetsInRange(
            projectile.transform,
            LayerMask.GetMask("HitBox"),
            projectile.Profile.BaseData.ExplosiveRange);
    }

    public void StartAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Hit");
    }

    public void CanselExplosion()
    {
        gameObject.SetActive(false);
    }

    public void OnCancelHitAnimation()
    {
        CanselExplosion();
        OnCanceled?.Invoke();
    }
}
