using UnityEngine;

public interface IBuildableOnTilemap
{
    public Vector3Int Size { get; set; }

    public BuildingState CurrentState { get; set; }
}

