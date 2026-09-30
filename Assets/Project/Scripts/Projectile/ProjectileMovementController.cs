using Zenject;

public class ProjectileMovementController : ITickable
{
    private readonly ProjectileContainer _projectileContainer;

    public ProjectileMovementController(ProjectileContainer projectileContainer)
    {
        _projectileContainer = projectileContainer;
    }

    public void Tick()
    {
        foreach (var projectileMovement in _projectileContainer.ProjectileMovements)
        {
            projectileMovement.MoveBall();
        }
    }
}
