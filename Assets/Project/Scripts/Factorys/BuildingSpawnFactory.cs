using UnityEngine;
using Zenject;


public class BuildingSpawnFactory
{
    private readonly DiContainer _container;
    private readonly TileOccupancyMap _tileOccupancyMap;

    public BuildingSpawnFactory(DiContainer container, TileOccupancyMap tileOccupancyMap)
    {
        _container = container;
        _tileOccupancyMap = tileOccupancyMap;
    }

    public TurretBuilding SpawnTurretBuilding(
        TurretBuildingProfile turretProfile, Vector3Int position, Transform parentForTurret, Transform parentForTurretHealthBar)
    {
        turretProfile.Data.Health.CurrentValue = turretProfile.Data.Health.StartValue;

        var go = _container.InstantiatePrefab(turretProfile.TurretBuildingPrefab, position, Quaternion.identity, parentForTurret);
        var go2 = _container.InstantiatePrefab(turretProfile.TurretHealthBarPrefab, position, Quaternion.identity, parentForTurretHealthBar);

        go.name = "Turret " + Time.frameCount;

        var turret = go.GetComponentInChildren<TurretBuilding>();
        var healthBar = go2.GetComponentInChildren<HealthBar>();

        go2.transform.localScale = Vector3.one;
        healthBar.transform.localScale = Vector3.one * 150;
        healthBar.Initialize(turret, turret.Health);

        healthBar.DisplayPoint = turret.HealthBarDisplayPoint;

        turret.EnemyContext = go;
        turret.HealthBar = healthBar;
        turret.TurretProfile = turretProfile;

        turret.Initialize();

        healthBar.DisplayPoint = turret.HealthBarDisplayPoint;

        return turret;
    }

    public bool ConfirmBuildingPlacement(Building building, Vector3Int positionInTilemap)
    {
        if (!_tileOccupancyMap.OccupyArea(positionInTilemap, building.Profile.BaseData.Size))
        {
            return false;
        }

        return true;
    }
}

