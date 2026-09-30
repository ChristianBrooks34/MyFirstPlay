using UnityEngine;

public abstract class Container<T> : MonoBehaviour
{
    public abstract void Add(T go);

    public abstract void Remove(T go);
}
