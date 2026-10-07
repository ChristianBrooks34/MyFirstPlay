using UnityEngine;
using Zenject;

public class BuildingInstaller : MonoInstaller
{
    [SerializeField] private Building _building;

    public override void InstallBindings()
    {
        Container
            .Bind<BuildingSpawnFactory>()
            .AsSingle();

        Container
            .Bind<Building>()
            .FromInstance(_building)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<BuildingEventManager>()
            .AsSingle();

        Container
            .Bind<BuildingStatusEffectManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<DamageFlash>()
            .AsSingle();

        Container
            .Bind<Health>()
            .FromInstance(new Health(_building.Profile))
            .AsSingle();
    }
}
