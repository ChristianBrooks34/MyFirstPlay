using System.Collections.Generic;

public class EnemyMoveContainer : Container<BaseMove>
{
    public List<BaseMove> EnemyMoves { get; private set; } = new List<BaseMove>();

    public override void Add(BaseMove go)
    {
        if (!EnemyMoves.Contains(go))
        {
            EnemyMoves.Add(go);
        }
    }

    public override void Remove(BaseMove go)
    {
        if (EnemyMoves.Contains(go))
        {
            EnemyMoves.Remove(go);
        }
    }
}
