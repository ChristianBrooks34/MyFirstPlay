using UnityEngine;

public interface IObjectPool
{
    public GameObject GetObject();
    public void ReturnObject(GameObject go);
}
