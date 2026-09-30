using UnityEngine.Tilemaps;
using Zenject;

public class TileMapClick : ITickable
{
    private Tilemap _map;
    private TileOccupancyMap _tileOccupancyMap;
    private CursorTileLocator _cursorTileLocator;
    private BuildingSpawner _buildingSpawner;
    private BuildingProfile _buildingProfile;

    public TileMapClick(Tilemap map, TileOccupancyMap tileOccupancyMap, CursorTileLocator cursorTileLocator,
        BuildingSpawner buildingSpawner, BuildingProfile buildingProfile)
    {
        _map = map;
        _buildingProfile = buildingProfile;
        _tileOccupancyMap = tileOccupancyMap;
        _cursorTileLocator = cursorTileLocator;
        _buildingSpawner = buildingSpawner;
    }

    public void Tick()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3Int? tilePosition = _cursorTileLocator.GetTilePositionUnderCursor();

        //    if (!tilePosition.HasValue)
        //    {
        //        Debug.Log(" урсор не над тайлом Ч спавн отменЄн.");
        //        return;
        //    }

        //    Vector3Int resultPos = new Vector3Int(tilePosition.Value.x, tilePosition.Value.y, 0);

        //    if (_map.GetTile(resultPos) == null) return;

        //    _buildingSpawner.StartBuildingPlacement(_buildingProfile, resultPos);
        //}
    }
}

