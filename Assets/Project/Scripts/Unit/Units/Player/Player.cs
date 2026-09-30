using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit, IKnockbackable, IPicked, IDisposable, IDamageable
{
    [NonSerialized] public GameObject PlayerContext;
    [NonSerialized] public HealthBar HealthBar;

    public Transform PointDamageBloodSplash;
    public Weapon Weapon;
    
    protected PlayerEventManager playerEventManager;
    protected LevelEventManager    levelEventManager;
    public List<BaseAttack> BaseAttacks { get; protected set; }
    public override float CalculateTotalDamage()
    {
        if (CurrentAttack == null) return UnitProfile.BaseData.Damage.CurrentValue;

        return UnitProfile.BaseData.Damage.CurrentValue + CurrentAttack.AttackData.Damage;
    }

    public virtual void Initialize(PlayerProfile playerProfile)
    {
        base.Initialize(playerProfile);

        UnitProfile = playerProfile;
        UnitData = playerProfile.Data;

        playerEventManager.TriggerPlayerInitialize(this);

        Health = new Health(UnitData);

        HealthBar.Initialize(this, Health);
    }

    public virtual void ApplyDamage(float damage)
    {
        Debug.LogError("ApplyDamage");
        var resultDamage = Mathf.Round(damage);

        if (Health.CurrentValue - resultDamage <= 0)
        {
            Health.Substract(resultDamage);

            Dead();
            return;
        }

        Health.Substract(resultDamage);

        playerEventManager.TriggerDamage(resultDamage);
    }

    public virtual void Dead()
    {
        if (IsDead) return;

        IsDead = true;

        GetComponent<Collider2D>().isTrigger = true;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        if (Weapon != null) Weapon.gameObject.SetActive(false);

        levelEventManager.TriggerDeadPlayer(this);
        playerEventManager.TriggerDead(this);

        DeadRoutine(
            () => gameObject.SetActive(false),
            UnitProfile.BaseData.DeadCooldown)
            .Forget();
    }

    public void TryPick(IPickable pickable)
    {
        pickable.OnPicked();
    }

    public void Dispose()
    {
        playerEventManager.OnPickableEnter -= TryPick;
    }

    public virtual bool TryApplyDamage(float damage)
    {
        Debug.LogError("TryApplyDamage");
        if (!CanBeAttacked) return false;

        ApplyDamage(damage);

        return true;
    }
}
