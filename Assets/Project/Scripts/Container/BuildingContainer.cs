using System.Collections.Generic;

public class BuildingContainer : Container<Building>
{
    public List<Building> Buildings { get; private set; } = new List<Building>();

    public override void Add(Building go)
    {
        if (go == null) return;

        Buildings.Add(go);

        go.transform.SetParent(transform);
    }

    public override void Remove(Building go)
    {
        if (go == null) return;
        if (!Buildings.Contains(go)) return;

        Buildings.Remove(go);

        go.transform.SetParent(null);
    }
}
