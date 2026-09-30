using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUtils : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    public (Vector3 min, Vector3 max) GetTilemapWorldBounds()
    {
        if (_tilemap == null)
        {
            _tilemap = GetComponent<Tilemap>();
            if (_tilemap == null)
                throw new System.NullReferenceException("Tilemap не найден!");
        }

        BoundsInt cellBounds = _tilemap.cellBounds;
        Vector3 cellSize = _tilemap.cellSize;

        Vector3 worldMin = _tilemap.CellToWorld(new Vector3Int(cellBounds.xMin, cellBounds.yMin, cellBounds.zMin));
        Vector3 worldMax = _tilemap.CellToWorld(new Vector3Int(cellBounds.xMax, cellBounds.yMax, cellBounds.zMax));

        worldMax -= cellSize;

        return (worldMin, worldMax);
    }

    private void OnDrawGizmos()
    {
        var (min, max) = GetTilemapWorldBounds();

        Gizmos.color = Color.green;
        DrawWireBounds(min, max);
    }

    private void DrawWireBounds(Vector3 min, Vector3 max)
    {
        Vector3 center = (min + max);
        Vector3 size = max - min;

        Gizmos.DrawWireCube(center, size);
    }
}
