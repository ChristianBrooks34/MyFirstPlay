using UnityEngine;
using Zenject;

public class CoreLevelInstaller : MonoInstaller
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private LevelProfile _level;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private LayerMask _flipableSprites;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private DamageRules _damageRules;
    [SerializeField] private AnimationCurvesConfig _animationCurvesConfig;


    private SpawnUnitEventManager _spawnUnitEventManager;

    public override void InstallBindings()
    {
        _spawnUnitEventManager = new SpawnUnitEventManager();

        _spawnUnitEventManager.OnPlayerSpawn += BindPlayer;

        Container
            .Bind<LevelController>()
            .FromInstance(_levelController)
            .AsCached()
            .NonLazy();

        Container
            .Bind<LevelProfile>()
            .FromInstance(_level)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<LevelEventManager>()
            .AsCached();

        Container
            .BindInterfacesTo<LevelEventManager>()
            .AsCached()
            .NonLazy();

        Container
            .Bind<AnimationCurvesConfig>()
            .FromInstance(_animationCurvesConfig)
            .AsSingle();

        Container
            .Bind<VisualEffectManager>()
            .AsSingle();

        Container
            .Bind<DamageRules>()
            .FromInstance(_damageRules)
            .AsSingle();

        Container
            .Bind<Camera>()
            .FromInstance(_camera)
            .AsSingle();
        Container

            .Bind<float>()
            .FromInstance(_lerpSpeed)
            .AsSingle();

        Container
            .Bind<LayerMask>()
            .FromInstance(_flipableSprites)
            .AsSingle();

        Container
            .Bind<AnimationCurve>()
            .FromInstance(_animationCurve)
            .AsSingle();

        Container
            .Bind<SpawnUnitFactory>()
            .AsSingle();

        Container
            .Bind<SpawnUnitEventManager>()
            .FromInstance(_spawnUnitEventManager)
            .AsSingle();

        Container
            .Bind<FlipController>()
            .AsSingle();

        Container
            .Bind<TargetLocator>()
            .AsSingle();

        Container
            .BindInterfacesTo<EnemyMoveController>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesTo<ProjectileMovementController>()
            .AsSingle()
            .NonLazy();
    }

    private void BindPlayer(Player player)
    {
        Container
            .Bind<Player>()
            .FromInstance(player)
            .AsSingle();

        Container
            .Bind<CameraFollow>()
            .AsSingle()
            .NonLazy();
    }

}