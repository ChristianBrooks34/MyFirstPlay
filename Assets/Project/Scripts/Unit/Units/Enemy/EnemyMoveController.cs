using Zenject;

public class EnemyMoveController : ITickable
{
    private readonly EnemyMoveContainer _enemyMoveContainer;

    public EnemyMoveController(EnemyMoveContainer enemyMoveContainer)
    {
        _enemyMoveContainer = enemyMoveContainer;
    }

    public void Tick()
    {
        foreach (var enemyMove in _enemyMoveContainer.EnemyMoves)
        {
            enemyMove.Move();
        }
    }
}
