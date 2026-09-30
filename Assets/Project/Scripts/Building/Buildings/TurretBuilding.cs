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

    public Health Health { get; set; }

    private BuildingEventManager _buildingEventManager;
    private bool _isDead;

    [Inject]
    public void Construct(BuildingEventManager buildingEventManager)
    {
        _buildingEventManager = buildingEventManager;
    }

    public void Initialize()
    {
        Health = new Health(TurretProfile);

        Health.Initialize(TurretProfile.Data.Health.StartValue);

        HealthBar.Initialize(this, Health);

        _buildingEventManager.TriggetInitialize();

        _isDead = false;
    }

    private void Update()
    {
        if (Target != null)
        {
            Rotation.RotateTowardsTarget(_centerRotate, Target.transform, TurretProfile.Data.SpeedRotate);
        }
    }

    public void ApplyDamage(float damage)
    {
        if (_isDead) return;

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

    public void Dead()
    {
        _isDead = true;

        Destroy(gameObject);
        Destroy(HealthBar.gameObject);
    }

    public float CalculateTotalDamage()
    {
        return TurretProfile.Data.Damage.CurrentValue;
    }

    public bool TryApplyDamage(float damage)
    {
        if (damage < 0) return false;

        ApplyDamage(damage);

        return true;
    }
}

