using UnityEngine;
using Zenject;

public class WeaponInstaller : MonoInstaller
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public override void InstallBindings()
    {
        Container
            .Bind<Weapon>()
            .FromInstance(_weapon)
            .AsSingle();

        Container
            .Bind<SpriteRenderer>()
            .FromInstance(_spriteRenderer)
            .AsSingle();
    }
}