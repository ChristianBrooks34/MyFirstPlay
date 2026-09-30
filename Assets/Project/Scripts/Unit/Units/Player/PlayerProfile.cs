using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Player")]
public class PlayerProfile : UnitProfile
{
    [SerializeField] private PlayerData _playerData;

    public GameObject SwordChargeBarPrefab;

    public override void InitializeUnitData()
    {
        if (BaseData != null)
        {
            return;
        }

        BaseData = Data;
    }

    public PlayerData Data
    {
        get => _playerData;
        set
        {
            if (value == null) return;
            _playerData = value;
            BaseData = value;
        }
    }
}

[Serializable]
public class PlayerData : UnitData
{
    public int LevelUpExp;
    public int StartLevelUpExp;
    public float LevelUpExpMultiplayer;
    public float SpeedRegeneration;
    public WeaponProfile WeaponProfile;
}

