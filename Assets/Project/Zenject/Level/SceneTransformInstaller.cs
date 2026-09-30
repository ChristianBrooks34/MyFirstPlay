using UnityEngine;
using Zenject;

public class SceneTransformInstaller : MonoInstaller
{
    [SerializeField] private Transform _parentForTurret;
    [SerializeField] private Transform _parentForTurretHealthBar;

    public override void InstallBindings()
    {
        Container.Bind<Transform>()
            .WithId(nameof(SpawnTurretAttack) + "parentForTurret")
            .FromInstance(_parentForTurret)
            .AsCached();

        Container.Bind<Transform>()
            .WithId(nameof(SpawnTurretAttack) + "parentForTurretHealthBar")
            .FromInstance(_parentForTurretHealthBar)
            .AsCached();
    }
}

