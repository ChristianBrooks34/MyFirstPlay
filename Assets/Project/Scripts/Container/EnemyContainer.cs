using System.Collections.Generic;

public class EnemyContainer : Container<Enemy>
{
    public List<Enemy> Enemy { get; private set; } = new List<Enemy>();

    public override void Add(Enemy go)
    {
        Enemy.Add(go);
    }

    public override void Remove(Enemy go)
    {
        if (Enemy.Contains(go))
        {
            Enemy.Remove(go);
        }
    }
}
