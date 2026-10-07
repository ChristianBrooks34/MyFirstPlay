using UnityEngine;

public class Building : MonoBehaviour, ITargetInfo, IDamageable
{
    public BuildingProfile Profile;

    [SerializeField] private EntityType _type = EntityType.Building;
    public EntityType Type => _type;

    public bool IsDead { get; private set; }
    public BuildingState CurrentState { get; set; }
    public Unit SourceUnit { get; set; }
    public bool CanBeAttacked { get; set; } = true;
    public Health Health { get; set; }

    public GameObject PlacementAllowedPanel;
    public GameObject CollisionFreePanel;

    public virtual void Initialize()
    {
        IsDead = false;
    }

    public virtual void ApplyDamage(float damage)
    {
        if (IsDead) return;

        var resultDamage = Mathf.Round(damage);

        if (Health.CurrentValue - resultDamage <= 0)
        {
            Health.Substract(resultDamage);

            Dead();
            return;
        }

        Health.Substract(resultDamage);
    }

    public virtual void Dead()
    {
        IsDead = true;
    }


    public virtual bool TryApplyDamage(float damage)
    {
        if (damage < 0) return false;
        if (!CanBeAttacked) return false;

        ApplyDamage(damage);

        return true;
    }
}
