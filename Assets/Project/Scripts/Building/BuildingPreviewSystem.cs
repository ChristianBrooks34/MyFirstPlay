using UnityEngine;

public class BuildingPreviewSystem
{
    private readonly CursorTileLocator _cursorTileLocator;
    private readonly TileOccupancyMap _tileOccupancyMap;

    public BuildingPreviewSystem(CursorTileLocator cursorTileLocator, TileOccupancyMap tileOccupancyMap)
    {
        _cursorTileLocator = cursorTileLocator;
        _tileOccupancyMap = tileOccupancyMap;
    }

    public void UpdatePreviewPosition(Building building)
    {
        var position = _cursorTileLocator.GetTileWorldPositionUnderCursor();

        if (position.HasValue)
        {
            building.transform.position =
                new Vector3(position.Value.x + 0.5f, position.Value.y + 0.5f, building.transform.position.z);

            UpdatePreviewVisuals(building);
        }
    }

    public bool CanPlaceBuilding(Building building)
    {
        var cursorTilePos = _cursorTileLocator.GetTilePositionUnderCursor();
        if (!cursorTilePos.HasValue) return false;

        Vector2Int buildingSize = building.Profile.BaseData.Size;
        return _tileOccupancyMap.IsAreaFree(cursorTilePos.Value, buildingSize);
    }

    public void UpdatePreviewVisuals(Building building)
    {
        if (CanPlaceBuilding(building))
        {
            building.PlacementAllowedPanel.SetActive(false);
            building.CollisionFreePanel.SetActive(true);
        }
        else
        {
            building.CollisionFreePanel.SetActive(false);
            building.PlacementAllowedPanel.SetActive(true);
        }
    }

    public void StartPreview(Building building)
    {
        building.CurrentState = BuildingState.Planned;

        building.GetComponent<BoxCollider2D>().enabled = false;

        UpdatePreviewVisuals(building);
    }

    public void StopPreview(Building building)
    {
        building.CurrentState = BuildingState.Completed;

        building.PlacementAllowedPanel.SetActive(false);
        building.CollisionFreePanel.SetActive(false);

        building.GetComponent<BoxCollider2D>().enabled = true;
    }
}
