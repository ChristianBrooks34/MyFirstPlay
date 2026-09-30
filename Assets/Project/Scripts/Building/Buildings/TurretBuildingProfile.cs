using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Turret")]
public class TurretBuildingProfile : BuildingProfile
{
    [SerializeField] private TurretBuildingData _turretBuildingData;

    public GameObject TurretBuildingPrefab;
    public GameObject TurretHealthBarPrefab;

    public TurretBuildingData Data
    {
        get
        {
            return _turretBuildingData;
        }
        set
        {
            if (value == null) return;
            _turretBuildingData = value;
        }
    }
}

[Serializable]
public class TurretBuildingData : BuildingData
{
    public float SpeedRotate;
    public float RadiusAttack;
    public float AttackCooldownInSeconds;
}

