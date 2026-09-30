using UnityEngine;
using Zenject;

public class BuildingSpawner
{
    private readonly BuildingContainer _buildingContainer;
    private readonly DiContainer _diContainer;

    public BuildingSpawner(BuildingContainer buildingContainer, DiContainer diContainer)
    {
        _buildingContainer = buildingContainer;
        _diContainer = diContainer;
    }

    public Building StartBuildingPlacement(BuildingProfile buildingProfile, Vector3Int positionInTilemap)
    {
        var go = _diContainer.InstantiatePrefab(buildingProfile.BuildingPrefab, positionInTilemap, Quaternion.identity,
            _buildingContainer.transform);

        var building = go.GetComponent<Building>();

        _buildingContainer.Add(building);

        return building;
    }
}
