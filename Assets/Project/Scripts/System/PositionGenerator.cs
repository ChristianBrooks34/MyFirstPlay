using System.Collections.Generic;
using UnityEngine;

public static class PositionGenerator
{
    public static List<Vector3> GetPointInCircle(int count, float radius, Vector3 spawnCenter)
    {
        List<Vector3> spawnPoints = new List<Vector3>();

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = spawnCenter + new Vector3(x, z, 0);

            spawnPoints.Add(spawnPosition);
        }

        return spawnPoints;
    }
}