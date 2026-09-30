using System;
using UnityEngine;

public abstract class Enemy : Unit, IKnockbackable, IDamageable
{
    [NonSerialized] public GameObject EnemyContext;
    [NonSerialized] public Player Target;
    [NonSerialized] public HealthBar HealthBar;
    [NonSerialized] public bool IsEnemyMoveRegisteredInContainer;

    public bool IsStaticEnemy;
    public Transform HealthBarDisplayPoint;
    public Transform PointDamageBloodSplash;
    public Transform PointDeadBloodSplash;
    public TriggerChecker TriggerChecker;

    public bool IsDisposable { get; private set; }

    protected abstract IObjectPool ObjectPool { get; set; }
    protected abstract UnitEventManager UnitEventManager { get; set; }
    protected abstract GlobalEventManager GlobalEventManager { get; set; }
    protected abstract IStateMachine StateMachine { get; set; }

    public virtual void Initialize(EnemyProfile enemyProfile)
    {
        base.Initialize(enemyProfile);

        UnitProfile = enemyProfile;
        UnitData = enemyProfile.Data;

        Health = new Health(UnitData);  
        Health.Initialize(UnitProfile.BaseData.Health.StartValue);

        HealthBar.Initialize(this, Health);

        StateMachine.ChangeState(EnemyState.Idle);
        IsDead = false;

        ActiveComponents();

        SetActive(true);

        IsDisposable = false;
    }

    public virtual void RecycleActivate()
    {
        SetActive(true);
        CanAttack = true;
        IsDead = false;

        Health.Initialize(UnitProfile.BaseData.Health.StartValue);

        HealthBar.Initialize(this, Health);

        ActiveComponents();

        StateMachine.ChangeState(EnemyState.Idle);
    }

    public virtual void RecycleDeactivate()
    {
        CanAttack = false;
        DisableComponents();
        SetActive(false);
    }

    public virtual void ApplyDamage(float damage)
    {
        if (StateMachine.IsCurrentState(EnemyState.Dead)) return;
        if (!CanBeAttacked) return;

        var resultDamage = Mathf.Round(damage);

        if (Health.CurrentValue - resultDamage <= 0)
        {
            Health.Substract(resultDamage);

            Dead();
            return;
        }

        Health.Substract(resultDamage);

        UnitEventManager.TriggerDamage(resultDamage);
    }

    public virtual void Dead()
    {
        if (StateMachine.IsCurrentState(EnemyState.Dead) || IsDead) return;
        IsDead = true;

        DisableComponents();

        GlobalEventManager.TriggerDeadEnemy(this);
        UnitEventManager.TriggerDead(this);

        DeadRoutine(
            () => ObjectPool.ReturnObject(gameObject),
            UnitProfile.BaseData.DeadCooldown)
            .Forget();
    }

    public override float CalculateTotalDamage()
    {
        return UnitProfile.BaseData.Damage.CurrentValue;
    }

    private void OnDestroy()
    {
        IsDisposable = true;
    }

    public void SetActive(bool flag)
    {
        HealthBar.gameObject.SetActive(flag);
        transform.parent.gameObject.SetActive(flag);
    }

    protected virtual void DisableComponents()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        HealthBar.gameObject.SetActive(false);
    }

    protected virtual void ActiveComponents()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;

        HealthBar.gameObject.SetActive(true);
    }

    public virtual bool TryApplyDamage(float damage)
    {
        if (!CanBeAttacked) return false;
        ApplyDamage(damage);

        return true;
    }
}
