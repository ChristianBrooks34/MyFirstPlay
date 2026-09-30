using UnityEngine;
using Zenject;

public class AttackSlotInstaller : MonoInstaller
{
    [SerializeField] private GameObject _attackSlotPrefab;
    [SerializeField] private Transform _parentForAttackCooldawnBar;

    public override void InstallBindings()
    {
        BindDisplayValueBar();

        Container
            .Bind<GameObject>()
            .FromInstance(_attackSlotPrefab)
            .AsCached();

        Container
            .Bind<Transform>()
            .FromInstance(_parentForAttackCooldawnBar)
            .AsCached();

        Container
            .Bind<SpawnerAttackSlot>()
            .AsSingle()
            .NonLazy();
    }

    private void BindDisplayValueBar()
    {
        var attackSlot = _attackSlotPrefab.GetComponent<AttackSlot>();

        Container
            .Bind<DisplayValueBar>()
            .AsSingle()
            .WithArguments(attackSlot.ImageAttackCooldawnBar, attackSlot.AttackCooldawnBar)
            .NonLazy();
    }
}