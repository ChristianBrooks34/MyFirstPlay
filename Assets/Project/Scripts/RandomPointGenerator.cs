using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPointGenerator
{
    private static Vector2 _bottomLeft;
    private static Vector2 _topRight;
    private static Vector2 _center;

    public static Vector2 GetRandomPointOutsideScreenBorder(float distanceBorder)
    {
        CalculateCameraBounds();

        Vector2 resultPoint = Vector2.zero;

        var direction = Random.Range(0, 4);

        switch (direction)
        {
            case 0:
                resultPoint = new Vector2(
                    _topRight.x + distanceBorder + Random.Range(-1f, 1f),
                    Random.Range(_bottomLeft.y - distanceBorder, _topRight.y + distanceBorder)
                );
                break;

            case 1:
                resultPoint = new Vector2(
                    _bottomLeft.x - distanceBorder + Random.Range(-1f, 1f),
                    Random.Range(_bottomLeft.y - distanceBorder, _topRight.y + distanceBorder)
                );
                break;

            case 2:
                resultPoint = new Vector2(
                    Random.Range(_bottomLeft.x - distanceBorder, _topRight.x + distanceBorder),
                    _topRight.y + distanceBorder + Random.Range(-1f, 1f)
                );
                break;

            case 3:
                resultPoint = new Vector2(
                    Random.Range(_bottomLeft.x - distanceBorder, _topRight.x + distanceBorder),
                    _bottomLeft.y - distanceBorder + Random.Range(-1f, 1f)
                );
                break;
        }

        return resultPoint;
    }


    public static List<Vector3> GetRandomPointOnCircle(int count, float radius, Vector2 center = default)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, 360f);
            float radians = angle * Mathf.Deg2Rad;

            float x = center.x + radius * Mathf.Cos(radians);
            float y = center.y + radius * Mathf.Sin(radians);

            result.Add(new Vector2(x, y));
        }

        return result;
    }

    private static void CalculateCameraBounds()
    {
        var camera = Camera.main;
        float height = camera.orthographicSize * 2;
        float width = height * camera.aspect;

        _center = camera.transform.position;

        _bottomLeft = _center + new Vector2(-width / 2, -height / 2);
        _topRight = _center + new Vector2(width / 2, height / 2);
    }

    public static Vector3 GetRandomPointOnTilemapAdaptive(Tilemap tilemap, Camera camera, Vector3 playerPosition)
    {
        SpawnZone spawnZone = AdaptiveSpawnZoneAnalyzer.GetAdaptiveSpawnZone(tilemap, camera, playerPosition);

        const int maxAttempts = 100;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            int randomX = Random.Range(spawnZone.XMin, spawnZone.XMax);
            int randomY = Random.Range(spawnZone.YMin, spawnZone.YMax);

            Vector3Int tilePos = new Vector3Int(randomX, randomY, 0);

            if (tilemap.HasTile(tilePos))
            {
                return tilemap.CellToWorld(tilePos);
            }
        }

        // Резервный вариант: центр карты
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int centerTile = new Vector3Int((bounds.min.x + bounds.max.x) / 2,
                                       (bounds.min.y + bounds.max.y) / 2, 0);
        return tilemap.CellToWorld(centerTile);
    }

    public static Vector3 GetRandomPointOutsideCameraOnTile(Tilemap tilemap, float distanceBorder = 5f)
    {
        CalculateCameraBounds();

        const int maxAttempts = 100;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 pointOutside = GetRandomPointOutsideScreenBorder(distanceBorder);
            Vector3Int tilePos = tilemap.WorldToCell(pointOutside);

            if (tilemap.HasTile(tilePos))
            {
                return tilemap.CellToWorld(tilePos);
            }
        }

        // Резервный вариант: случайный тайл на карте
        BoundsInt bounds = tilemap.cellBounds;
        int randomX = Random.Range(bounds.min.x, bounds.max.x);
        int randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector3Int randomTilePos = new Vector3Int(randomX, randomY, 0);

        return tilemap.HasTile(randomTilePos)
            ? tilemap.CellToWorld(randomTilePos)
            : Vector3.zero;
    }
}
