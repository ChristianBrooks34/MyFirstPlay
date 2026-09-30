using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorTileLocator
{
    public Tilemap Tilemap { get; private set; }
    private Camera _camera;

    public CursorTileLocator(Tilemap tilemap, Camera camera)
    {
        Tilemap = tilemap;

        _camera = camera;
    }

    public Vector3Int GetTilePosition(Vector3 tilePos)
    {
        return Tilemap.WorldToCell(tilePos);
    }

    public Vector3Int? GetTilePositionUnderCursor()
    {
        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        Vector3Int tilePosition = GetTilePosition(mouseWorldPosition);

        if (Tilemap.GetTile(tilePosition) == null)
        {
            return null;
        }
        return tilePosition;
    }

    public Vector3? GetTileWorldPositionUnderCursor()
    {
        var tilePosition = GetTilePositionUnderCursor();

        if (!tilePosition.HasValue)
            return null;

        var worldPosition = Tilemap.CellToWorld(tilePosition.Value);
        return worldPosition;
    }

    public bool IsCursorOverTile()
    {
        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        return HasTileAtWorldPosition(mouseWorldPosition);
    }

    public bool HasTileAtWorldPosition(Vector3 worldPosition)
    {
        Vector3Int tilePosition = Tilemap.WorldToCell(worldPosition);

        return Tilemap.GetTile(tilePosition) != null;
    }
}
