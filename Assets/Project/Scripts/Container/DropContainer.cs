using UnityEngine;

public class DropContainer : Container<GameObject>
{
    public override void Add(GameObject go)
    {
        go.transform.SetParent(transform);
    }

    public override void Remove(GameObject go)
    {
        go.transform.SetParent(null);
    }
}
