using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SwordChargeBarInstaller : MonoInstaller
{
    [SerializeField] private SwordChargeBar _swordChargeBar;
    [SerializeField] private Image _imageSwordChargeBar;
    [SerializeField] private GameObject _parentHealthBar;
    [SerializeField] private PlayerChargeAttack _playerChargeAttack;

    public override void InstallBindings()
    {
        Container
            .Bind<ValueBar>()
            .To<SwordChargeBar>()
            .FromInstance(_swordChargeBar)
            .AsSingle();

        Container
            .Bind<DisplayValueBar>()
            .AsCached();

        Container
            .BindInterfacesTo<DisplayValueBar>()
            .AsCached();

        Container
            .Bind<PlayerChargeAttack>()
            .FromInstance(_playerChargeAttack)
            .AsSingle();

        Container
            .Bind<Image>()
            .FromInstance(_imageSwordChargeBar)
            .AsSingle();

        Container
            .Bind<GameObject>()
            .FromInstance(_parentHealthBar)
            .AsSingle();
    }
}