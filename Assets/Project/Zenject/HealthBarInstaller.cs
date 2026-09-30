using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBarInstaller : MonoInstaller
{
    [SerializeField] private Image _imageDisplayValue;
    [SerializeField] private GameObject _parentHealthBar;
    [SerializeField] private Text _displayText;
    [SerializeField] private HealthBar _healthBar;

    public override void InstallBindings()
    {
        Container
            .Bind<Image>()
            .FromInstance(_imageDisplayValue)
            .AsSingle();

        Container
            .Bind<GameObject>()
            .FromInstance(_parentHealthBar)
            .AsSingle();

        Container
            .Bind<Text>()
            .FromInstance(_displayText)
            .AsSingle();

        Container
            .Bind<ValueBar>()
            .To<HealthBar>()
            .FromInstance(_healthBar)
            .AsSingle();

        Container
            .BindInterfacesTo<DisplayValueBar>()
            .AsCached();

        Container
            .Bind<DisplayValueBar>()
            .AsCached();
    }
}