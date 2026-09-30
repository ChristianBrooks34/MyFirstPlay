using UnityEngine;
using Zenject;

public class ContainersInstaller : MonoInstaller
{
    [SerializeField] private BulletsContainer _bulletContainer;
    [SerializeField] private ProjectileContainer _projectlileContainer;
    [SerializeField] private DropContainer _dropContainer;
    [SerializeField] private EnemyMoveContainer _enemyMoveContainer;
    [SerializeField] private BuildingContainer _buildingContainer;

    public override void InstallBindings()
    {
        Container
            .Bind<BulletsContainer>()
            .FromInstance(_bulletContainer)
            .AsSingle();

        Container
            .Bind<ProjectileContainer>()
            .FromInstance(_projectlileContainer)
            .AsSingle();

        Container
            .Bind<EnemyMoveContainer>()
            .FromInstance(_enemyMoveContainer)
            .AsSingle();

        Container
            .Bind<DropContainer>()
            .FromInstance(_dropContainer)
            .AsSingle();

        Container
            .Bind<BuildingContainer>()
            .FromInstance(_buildingContainer)
            .AsSingle();
    }
}
