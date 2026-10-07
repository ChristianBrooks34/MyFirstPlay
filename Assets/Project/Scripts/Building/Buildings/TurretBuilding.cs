using System;
using UnityEngine;
using Zenject;

public class TurretBuilding : Building, IDamageable // нужен рефакторинг
{
    [NonSerialized] public GameObject EnemyContext;
    [NonSerialized] public bool CanAttack;
    [NonSerialized] public HealthBar HealthBar;
    [NonSerialized] public Enemy Target;

    [SerializeField] private GameObject _centerRotate;

    public Transform HealthBarDisplayPoint;

    public TurretBuildingProfile TurretProfile;

    private BuildingEventManager _buildingEventManager;

    [Inject]
    public void Construct(BuildingEventManager buildingEventManager)
    {
        _buildingEventManager = buildingEventManager;
    }

    public override void Initialize()
    {
        base.Initialize();

        Health = new Health(TurretProfile);

        Health.Initialize(TurretProfile.Data.Health.StartValue);

        HealthBar.Initialize(this, Health);

        _buildingEventManager.TriggetInitialize();

    }

    private void Update()
    {
        if (Target != null)
        {
            Rotation.RotateTowardsTarget(_centerRotate, Target.transform, TurretProfile.Data.SpeedRotate);
        }
    }

    public override void ApplyDamage(float damage)
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

        _buildingEventManager.TriggetDamage(resultDamage);
    }

    public override bool TryApplyDamage(float damage)
    {
        if (damage < 0) return false;

        if (!CanBeAttacked) return false;

        ApplyDamage(damage);

        return true;
    }

    public override void Dead()
    {
        base.Dead();

        Destroy(gameObject);
        Destroy(HealthBar.gameObject);
    }

    public float CalculateTotalDamage()
    {
        return TurretProfile.Data.Damage.CurrentValue;
    }
}

