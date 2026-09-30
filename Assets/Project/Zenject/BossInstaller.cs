using UnityEngine;
using Zenject;

public class BossInstaller : MonoInstaller
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _damageableLayers;
    [SerializeField] private BaseMove _baseMove;
    [SerializeField] private BaseAttack _standartEnemyAttack;

    public override void InstallBindings()
    {
        Container
            .Bind<Enemy>()
            .FromInstance(_boss)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<Boss>()
            .FromInstance(_boss)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<Transform>()
            .FromInstance(_boss.transform)
            .AsSingle();

        Container
            .Bind<LayerMask>()
            .FromInstance(_damageableLayers)
            .AsSingle();

        Container
            .Bind<BossController>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<BaseMove>()
            .FromInstance(_baseMove)
            .AsCached();

        Container
            .Bind<BossAttackController>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<EnemyEffectManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<EnemyStateMachine>()
            .AsCached();

        Container
            .BindInterfacesTo<EnemyStateMachine>()
            .AsCached();

        Container
            .Bind<EnemyEventManager>()
            .AsSingle()
            .NonLazy();

        Container
             .Bind<UnitEventManager>()
             .AsSingle()
             .NonLazy();

        Container
             .Bind<BossEventManager>()
             .AsSingle()
             .NonLazy();

        Container
            .Bind<DamageFlash>()
            .AsSingle();

        Container
            .Bind<SpriteRenderer>()
            .FromInstance(_spriteRenderer)
            .AsSingle();

        Container
            .Bind<Animator>()
            .FromInstance(_enemyAnimator)
            .AsSingle();

        Container
            .BindInterfacesTo<EnemyAnimationController>()
            .AsCached();

        Container
            .Bind<EnemyController>()
            .AsCached();

        Container
            .BindInterfacesTo<DepthController>()
            .AsSingle()
            .WithArguments(_boss)
            .NonLazy();

        Container
            .Bind<LootDroper>()
            .AsSingle()
            .NonLazy();

        Container
           .Bind<BaseAttack>()
           .FromInstance(_standartEnemyAttack)
           .AsSingle()
           .NonLazy();
    }
}

