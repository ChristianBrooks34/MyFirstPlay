using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IUnit<UnitProfile, UnitData>, ITargetInfo
{
    [NonSerialized] public BaseAttack CurrentAttack;

    public Transform PointDropSpawn;

    [SerializeField] private EntityType _type = EntityType.Building;
    public EntityType Type => _type;
    public bool CanAttack { get; set; } = false;
    public bool IsDead { get; protected set; } = false;
    public Health Health { get; set; }
    public UnitProfile UnitProfile { get; set; }
    public UnitData UnitData { get; set; }

    public virtual float CalculateTotalDamage() { return 0; }

    public virtual void Initialize(UnitProfile unitData)
    {
        UnitProfile = unitData;
        UnitData = unitData.BaseData;
    }

    public async UniTaskVoid DeadRoutine(Action action, float seconds)
    {
        await UniTask.Delay((int)(seconds * 1000));

        if (this == null || gameObject == null)
            return;

        action?.Invoke();

        if (this == null || gameObject == null)
            return;

        UnitProfile.BaseData.Health.CurrentValue = UnitProfile.BaseData.Health.StartValue;
    }

    public float GetHealthInProcent()
    {
        return Health.CurrentValue / UnitProfile.BaseData.Health.StartValue;
    }
}
