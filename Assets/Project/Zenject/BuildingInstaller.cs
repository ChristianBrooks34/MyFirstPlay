using UnityEngine;
using Zenject;

public class BuildingInstaller : MonoInstaller
{
    [SerializeField] private Building _building;

    public override void InstallBindings()
    {
        BindAttack();

        Container
            .Bind<BuildingSpawnFactory>()
            .AsSingle();

        Container
            .Bind<Building>()
            .FromInstance(_building)
            .AsSingle()
            .NonLazy();

        //Container
        //    .Bind<Transform>()
        //    .FromInstance(_building.transform)
        //    .AsSingle();

        Container
            .Bind<DamageFlash>()
            .AsSingle();

        Container
            .Bind<Health>()
            .FromInstance(new Health(_building.Profile))
            .AsSingle();
    }

    public void BindAttack()
    {
        //Container
        //   .Bind<BaseAttack>()
        //   .To<StandartEnemyAttack>()
        //   .FromInstance(_standartEnemyAttack)
        //   .AsSingle()
        //   .NonLazy();
    }
}
