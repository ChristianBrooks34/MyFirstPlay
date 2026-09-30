using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AttackHitChecker
{
    public static List<GameObject> GetFrontTargetsInAttackRange(Transform center, LayerMask layerMask, float radiysAttack)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        var colliders = Physics2D.OverlapCircleAll(center.position, radiysAttack, 1 << layerMask);

        foreach (var collider in colliders)
        {
            var enemyDir = collider.transform.position - center.position;
            if (Vector3.Dot(enemyDir.normalized, center.localScale) > 0)
            {
                gameObjects.Add(collider.gameObject);
            }
        }

        return gameObjects;
    }

    public static List<GameObject> GetAllTargetsInRange(Transform center, LayerMask layerMask, float radiysAttack)
    {
        var colliders = Physics2D.OverlapCircleAll(center.position, radiysAttack, layerMask);

        return colliders.Select(x => x.gameObject).ToList();
    }
}
