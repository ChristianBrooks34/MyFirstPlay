using UnityEngine;
using Zenject;

public class TurretInstaller : MonoInstaller
{
    [SerializeField] private EnemyTurretProfile _turretProfile;
    [SerializeField] private Turret _turret;
    [SerializeField] private BulletTurretAttack _bulletTurretAttack;

    public override void InstallBindings()
    {
        Container
            .Bind<EnemyTurretProfile>()
            .FromInstance(_turretProfile)
            .AsSingle();

        Container
            .Bind<Turret>()
            .FromInstance(_turret)
            .AsSingle();

        Container
            .Bind<TurretEventManager>()
            .AsSingle();

        Container
            .Bind<TurretStateMachine>()
            .AsSingle();

        Container
            .BindInterfacesTo<TurretRotationController>()
            .AsSingle()
            .WithArguments(_turret, _turretProfile)
            .NonLazy();

        Container
            .Bind<BaseAttack>()
            .To<BulletTurretAttack>()
            .FromInstance(_bulletTurretAttack)
            .AsSingle()
            .NonLazy();
    }
}