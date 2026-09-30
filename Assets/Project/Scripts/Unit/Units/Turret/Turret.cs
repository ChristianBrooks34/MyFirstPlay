using System;
using UnityEngine;
using Zenject;

public class Turret : Enemy, IBuildingEnemy
{
    [SerializeField] private Vector2Int _size;
    [NonSerialized] public GameObject TuerretContext;

    public Transform PointBulletSpawn;
    public Transform CenterRotateHead;

    private TurretEventManager _turretEventManager;
    private TurretStateMachine _turretStateMachine;

    public Vector2Int Size
    {
        get => _size;
        set => _size = value;
    }

    protected override IObjectPool ObjectPool { get; set; }
    protected override GlobalEventManager GlobalEventManager { get; set; }
    protected override IStateMachine StateMachine { get; set; }
    protected override UnitEventManager UnitEventManager { get; set; }

    [Inject]
    public void Construct(TurretEventManager turretEventManager, EnemyPool enemyPool, TurretStateMachine turretStateMachine,
        GlobalEventManager globalEventManager, SpawnUnitEventManager spawnUnitEventManager)
    {
        ObjectPool = enemyPool;
        UnitEventManager = turretEventManager;
        StateMachine = turretStateMachine;
        _turretEventManager = turretEventManager;
        _turretStateMachine = turretStateMachine;
        GlobalEventManager = globalEventManager;

        spawnUnitEventManager.OnPlayerSpawn +=
            (player) =>
            {
                Target = player;
            };
    }

    public override void Initialize(EnemyProfile unitProfile)
    {
        UnitProfile = unitProfile;
        UnitData = unitProfile.Data;

        Health = new Health(UnitData);
        Health.Initialize(UnitProfile.BaseData.Health.StartValue);

        HealthBar.Initialize(this, Health);

        IsDead = false;

        ActiveComponents();

        SetActive(true);

        _turretEventManager.TriggerInitialize();
    }

    public override void Dead()
    {
        Debug.LogError($"Turret Dead 1");
        if (_turretStateMachine.CurrentState == TurretState.Dead || IsDead) return;
        IsDead = true;

        Debug.LogError($"Turret Dead 2");

        DisableComponents();

        GlobalEventManager.TriggerDeadEnemy(this);
        UnitEventManager.TriggerDead(this);

        DeadRoutine(
            () => ObjectPool.ReturnObject(gameObject),
            UnitProfile.BaseData.DeadCooldown)
            .Forget();
    }

    private void OnDestroy()
    {
        _turretEventManager.TriggerDisposable();
    }

    protected override void ActiveComponents()
    {
        GetComponent<BoxCollider2D>().enabled = true;

        HealthBar.gameObject.SetActive(true);
    }
    protected override void DisableComponents()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        HealthBar.gameObject.SetActive(false);
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

        //UnitEventManager.TriggerDamage(resultDamage);
    }
}
