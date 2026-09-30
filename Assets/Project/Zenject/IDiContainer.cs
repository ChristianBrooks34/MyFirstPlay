using UnityEngine;

public interface IDiContainer
{
    GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent);
    GameObject InstantiatePrefab(GameObject prefab, Transform parent);
}