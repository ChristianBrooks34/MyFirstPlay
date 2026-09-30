using Zenject;

public class BuildingSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<BuildingSpawner>()
            .AsSingle();

        Container
            .Bind<BuildingPreviewSystem>()
            .AsSingle();

        Container
            .Bind<BuildingSpawnFactory>()
            .AsSingle();
    }
}