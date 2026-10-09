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

    [Header("Configs")]
    [SerializeField] private AnimationCurvesConfig _curvesConfig;

    [Header("Drop Effects Assets")]
    [SerializeField] private VisualEffectTween _waveVerticalAsset;
    [SerializeField] private VisualEffectTween _pulsateScaleAsset;

    public override void InstallBindings()
    {
        BindEffects();

        BindObjectsPool();

        BindVisualEffect();
    }

    private void BindVisualEffect()
    {
        Container.BindInstance(_curvesConfig).AsSingle();

        Container.Bind<VisualEffectManager>().AsSingle();

        Container.Bind<VisualEffectTween>()
            .WithId(nameof(WaveVerticalEffect))
            .FromInstance(_waveVerticalAsset);

        Container.Bind<VisualEffectTween>()
            .WithId(nameof(PulsateScaleEffect))
            .FromInstance(_pulsateScaleAsset);
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
    }
}

