using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseEffectPool : IObjectPool
{
    protected DiContainer DiContainer;
    protected GameObject Prefab;
    protected Transform Parent;
    protected int PoolSize;

    private Queue<GameObject> _pool = new Queue<GameObject>();
    private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

    public GameObject GetObject()
    {
        if (_pool.Count == 0)
        {
            return null;
        }

        var go = _pool.Dequeue();

        if (go == null || go.gameObject == null)
        {
            Debug.LogWarning("Попытка получить уничтоженный объект !");
            return null;
        }

        go.SetActive(true);
        _activeObjects.Add(go);

        return go;
    }

    public void ReturnObject(GameObject go)
    {
        if (go == null || go.gameObject == null)
        {
            Debug.LogWarning("Попытка вернуть уничтоженный объект!");
            return;
        }

        if (_activeObjects.Contains(go))
        {
            go.SetActive(false);
            go.transform.SetParent(Parent);

            _pool.Enqueue(go);
            _activeObjects.Remove(go);
        }
        else
        {
            Debug.LogWarning("Объект не был активен!");
        }
    }

    protected void SpawnGameObjects()
    {
        if (Prefab == null) return;

        for (int i = 0; i < PoolSize; i++)
        {
            var go = DiContainer.InstantiatePrefab(Prefab, Parent);
            go.SetActive(false);
            _pool.Enqueue(go);
        }
    }
}
