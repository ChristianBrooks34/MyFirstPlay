using UnityEngine;
using Zenject;

[CreateAssetMenu(
    fileName = "ProjectInstaller",
    menuName = "Installers/ProjectInstaller"
)]

public class ProjectInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<Wallet>()
            .AsSingle();

        Container
            .Bind<Game>()
            .AsCached();

        Container
            .BindInterfacesTo<Game>()
            .AsCached();

        Container
            .Bind<JsonSaveSystem<PlayerProfile>>()
            .AsSingle();

        Container
            .Bind<GameSaveSystem>()
            .AsCached();

        Container
           .BindInterfacesTo<GameSaveSystem>()
           .AsCached();

        Container
            .Bind<EnemyCounter>()
            .AsCached();

        Container
            .BindInterfacesTo<EnemyCounter>()
            .AsCached();

        Container
            .Bind<GameLauncer>()
            .AsSingle();

        Container
            .Bind<ApplicationFinisher>()
            .AsSingle();

        Container
            .Bind<UnitProfileManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<BuildingEventManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<LevelProfileManager>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesTo<UnitLevelManager>()
            .AsCached();

        Container
            .Bind<UnitLevelManager>()
            .AsCached();

        Container
            .Bind<SpawnUnitForUIFactory>()
            .AsSingle();

        BindGlobalEventManager();
    }

    public void BindGlobalEventManager()
    {
        Container
            .Bind<GlobalEventManager>()
            .AsSingle();
    }
}