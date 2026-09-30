using UnityEngine;
using UnityEngine.Tilemaps;

public class TileOccupancyMap
{
    private Tilemap _tilemap;
    private bool[,] grid;
    private int width, height;
    private int xOffset, yOffset;

    public TileOccupancyMap(Tilemap tilemap)
    {
        _tilemap = tilemap;

        BoundsInt bounds = _tilemap.cellBounds;

        xOffset = bounds.xMin;
        yOffset = bounds.yMin;

        width = bounds.size.x;
        height = bounds.size.y;

        grid = new bool[width, height];

        InitializeFromTilemap();
    }

    public bool IsCellFree(Vector3Int worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        if (gridPos.x < 0 || gridPos.x >= width || gridPos.y < 0 || gridPos.y >= height)
            return false;

        return !grid[gridPos.x, gridPos.y];
    }

    public void OccupyCell(Vector3Int worldPos, bool occupied = true)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        if (gridPos.x > 0 && gridPos.x < width && gridPos.y > 0 && gridPos.y < height)
        {
            grid[gridPos.x, gridPos.y] = occupied;
        }
    }

    public void FreeCell(int x, int y) => OccupyCell(new Vector3Int(x, y, 0), false);

    public bool IsAreaFree(Vector3Int positionInTilemap, Vector2Int size)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector3Int checkPos = new Vector3Int(
                    positionInTilemap.x - x,
                    positionInTilemap.y + y,
                    0
                );

                if (!IsCellFree(checkPos))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool OccupyArea(Vector3Int positionInTilemap, Vector2Int size)
    {
        if (!IsAreaFree(positionInTilemap, size))
            return false;

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector3Int cellPos = new Vector3Int(
                    positionInTilemap.x - x,
                    positionInTilemap.y + y,
                    0
                );

                OccupyCell(cellPos);
            }
        }

        return true;
    }

    private void InitializeFromTilemap()
    {
        BoundsInt bounds = _tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                if (_tilemap.HasTile(cellPos))
                {
                    OccupyCell(cellPos, false);
                }
            }
        }
    }

    private Vector2Int WorldToGrid(Vector3Int worldPos)
    {
        return new Vector2Int(
            worldPos.x - xOffset,
            worldPos.y - yOffset
        );
    }

    private Vector3Int GridToWorld(Vector2Int gridPos)
    {
        return new Vector3Int(
            gridPos.x + xOffset,
            gridPos.y + yOffset,
            0
        );
    }
}
