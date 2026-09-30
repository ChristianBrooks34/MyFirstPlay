using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BaseAttack _standartEnemyAttack;
    [SerializeField] private LayerMask _damageableLayers;
    [SerializeField] private BaseMove _baseMove;

    public override void InstallBindings()
    {
        BindAttack();

        Container
            .Bind<Enemy>()
            .FromInstance(_enemy)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<Transform>()
            .FromInstance(_enemy.transform)
            .AsSingle();

        Container
            .Bind<LayerMask>()
            .FromInstance(_damageableLayers)
            .AsSingle();

        Container
            .Bind<BaseMove>()
            .FromInstance(_baseMove)
            .AsCached();

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
            .AsCached()
            .NonLazy();

        Container
            .Bind<UnitEventManager>()
            .AsCached();

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
            .AsCached()
            .NonLazy();

        Container
            .BindInterfacesTo<DepthController>()
            .AsSingle()
            .WithArguments(_enemy)
            .NonLazy();

        Container
            .Bind<LootDroper>()
            .AsSingle()
            .NonLazy();
    }

    public void BindAttack()
    {
        Container
           .Bind<BaseAttack>()
           .FromInstance(_standartEnemyAttack)
           .AsSingle()
           .NonLazy();
    }
}