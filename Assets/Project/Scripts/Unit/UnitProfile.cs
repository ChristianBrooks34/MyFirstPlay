using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitProfile : BaseProfile<UnitData>
{
    public override UnitData BaseData { get; set; }

    public GameObject UnitPrefabForUI;
    public GameObject UnitPrefab;
    public GameObject HealthBarPrefab;

    public abstract void InitializeUnitData();
}

public interface IUnit<TProfile, TData> where TProfile : UnitProfile where TData : class
{
    public TProfile UnitProfile { get; set; }
    public TData UnitData { get; set; }
}

[Serializable]
public class UnitData : ProgressableData
{
    public int CurrentLevel;

    public DevelopItemFloat Health = new DevelopItemFloat();
    public DevelopItemFloat Damage = new DevelopItemFloat();
    public DevelopItemFloat SpeedMovement = new DevelopItemFloat();

    public int BaseMaxHealth;
    public int MaxHealth;
    public float DeadCooldown;
    public KnockbackData KnockbackData;
    public UnitSize SizeType = UnitSize.Medium;

    public List<DropItem> DropItems;
}

[Serializable]
public class KnockbackData
{
    public float ForceAttack;
    public float KnockbackTime;
    public float KnockbackDistance;
}
