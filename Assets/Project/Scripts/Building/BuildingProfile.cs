using System;
using UnityEngine;

public class BuildingProfile : BaseProfile<BuildingData>
{
    [SerializeField] private BuildingData _buildingData;

    public GameObject BuildingPrefab;
    public override BuildingData BaseData
    {
        get
        {
            return _buildingData;
        }
        set
        {
            if (value == null) return;
            _buildingData = value;
        }
    }
}

[Serializable]
public class BuildingData : UnitData
{
    public Vector2Int Size;
}
