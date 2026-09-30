using System.Collections.Generic;
using UnityEngine;

public class TargetLocator
{
    public List<Unit> GetTargetsInRadius(Vector3 center, float radius, LayerMask targetLayerMask, Transform excluder)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, targetLayerMask);

        List<Unit> targets = new List<Unit>();

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.transform != excluder)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    targets.Add(unit);
                }
            }
        }

        return targets;
    }

    public Enemy GetNearestEnemy(Transform center, float radius)
    {
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        if (enemyLayerIndex == -1)
        {
            Debug.LogError("Слой 'Enemy' не найден!");
            return null;
        }

        LayerMask enemyLayerMask = 1 << enemyLayerIndex;
        var units = GetTargetsInRadius(center.position, radius, enemyLayerMask, center);

        Enemy nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var unit in units)
        {
            if (unit is Enemy enemy && enemy != null && enemy.gameObject != null && !enemy.IsDead)
            {
                var distance = Vector3.Distance(center.position, enemy.transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }
}
