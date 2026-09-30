using UnityEngine;
using Zenject;

public class LevelEffectsInstaller : MonoInstaller
{
    [SerializeField] private BloodSplashDamageEffect _bloodSplashDamageEffect;
    [SerializeField] private BloodSplashDeadEffect _bloodSplashDeadEffect;

    [SerializeField] private GameObject _damageBloodSplashEffectPoolPrefab;
    [SerializeField] private int _damageBloodSplashEffectPoolSize;
    [SerializeField] private Transform _parenForDamageBloodSplashEffect;

    [SerializeField] private GameObject _deadBloodSplashEffectPoolPrefab;
    [SerializeField] private int _deadBloodSplashEffectPoolSize;
    [SerializeField] private Transform _parenForDeadBloodSplashEffect;

    public override void InstallBindings()
    {
        BindEffects();
        BindObjectsPool();
    }

    private void BindEffects()
    {
        Container
            .Bind<BloodSplashDamageEffect>()
            .FromInstance(_bloodSplashDamageEffect)
            .AsSingle();

        Container
            .Bind<BloodSplashDeadEffect>()
            .FromInstance(_bloodSplashDeadEffect)
            .AsSingle();
    }

    private void BindObjectsPool()
    {
        Container
            .Bind<DamageBloodSplashEffectPool>()
            .AsSingle()
            .WithArguments(
                _damageBloodSplashEffectPoolPrefab,
                _damageBloodSplashEffectPoolSize,
                _parenForDamageBloodSplashEffect);

        Container
            .Bind<DeadBloodSplashEffectPool>()
            .AsSingle()
            .WithArguments(
                _deadBloodSplashEffectPoolPrefab,
                _deadBloodSplashEffectPoolSize,
                _parenForDeadBloodSplashEffect);

        Container
            .Bind<EnemyPool>()
            .AsSingle();
    }
}

