using System.Collections.Generic;

public class ProjectileContainer : Container<BaseProjectileMovement>
{
    public List<BaseProjectileMovement> ProjectileMovements { get; private set; } = new List<BaseProjectileMovement>();
    public override void Add(BaseProjectileMovement go)
    {
        ProjectileMovements.Add(go);

        go.gameObject.transform.SetParent(transform);
    }

    public override void Remove(BaseProjectileMovement go)
    {
        ProjectileMovements.Remove(go);

        go.gameObject.transform.SetParent(null);
    }
}
