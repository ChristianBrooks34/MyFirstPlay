using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile")]
public class ProjectileProfile : BaseProfile<ProjectileData>
{
    [SerializeField] private ProjectileData _projectileData;

    public override ProjectileData BaseData
    {
        get
        {
            return _projectileData;
        }
        set
        {
            if (value == null) return;
            _projectileData = value;
        }
    }
}

[Serializable]
public class ProjectileData : ProgressableData
{
    public float MaxDamage;
    public float MinDamage;
    public float Speed;
    public Unit SourceUnit;
    public AnimationCurve VecrticalMovementCurve;
    public float ExplosiveRange;
    public float TimeLiveInSeconds;
    public float MaxForseExplosive;
    public float MinForseExplosive;
}
