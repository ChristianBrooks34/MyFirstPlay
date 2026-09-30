using UnityEngine;
using UnityEngine.Tilemaps;

public static class AdaptiveSpawnZoneAnalyzer
{
    public static SpawnZone GetAdaptiveSpawnZone(Tilemap tilemap, Camera camera, Vector3 playerPosition)
    {
        var corners = GetCameraCorners(camera);
        bool[] hasTileInCorner = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            hasTileInCorner[i] = HasTileAtPosition(tilemap, corners[i]);
        }

        // Определяем границы зоны спавна на основе видимости углов и позиции игрока
        BoundsInt tilemapBounds = tilemap.cellBounds;
        int xMin = tilemapBounds.min.x;
        int xMax = tilemapBounds.max.x;
        int yMin = tilemapBounds.min.y;
        int yMax = tilemapBounds.max.y;

        Vector3Int playerTilePos = tilemap.WorldToCell(playerPosition);

        // Базовые границы — вся карта
        int spawnXMin = xMin, spawnXMax = xMax;
        int spawnYMin = yMin, spawnYMax = yMax;

        // Анализируем комбинации пустых углов и корректируем зону
        if (!hasTileInCorner[0] && !hasTileInCorner[1]) // Нижние углы пустые
        {
            spawnYMin = playerTilePos.y; // Нижняя граница — по Y игрока
        }
        if (!hasTileInCorner[2] && !hasTileInCorner[3]) // Верхние углы пустые
        {
            spawnYMax = playerTilePos.y; // Верхняя граница — по Y игрока
        }
        if (!hasTileInCorner[0] && !hasTileInCorner[2]) // Левые углы пустые
        {
            spawnXMin = playerTilePos.x; // Левая граница — по X игрока
        }
        if (!hasTileInCorner[1] && !hasTileInCorner[3]) // Правые углы пустые
        {
            spawnXMax = playerTilePos.x; // Правая граница — по X игрока
        }

        return new SpawnZone
        {
            XMin = spawnXMin,
            XMax = spawnXMax,
            YMin = spawnYMin,
            YMax = spawnYMax
        };
    }

    private static Vector3[] GetCameraCorners(Camera camera)
    {
        float height = camera.orthographicSize;
        float width = height * camera.aspect;
        Vector3 center = camera.transform.position;

        return new[]
        {
            center + new Vector3(-width, -height, 0), // нижний левый
            center + new Vector3(width, -height, 0),  // нижний правый
            center + new Vector3(-width, height, 0),   // верхний левый
            center + new Vector3(width, height, 0)    // верхний правый
        };
    }

    private static bool HasTileAtPosition(Tilemap tilemap, Vector3 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        return tilemap.HasTile(cellPos);
    }
}

// Структура для хранения зоны спавна
public struct SpawnZone
{
    public int XMin, XMax, YMin, YMax;
}
