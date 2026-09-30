using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameSystemInstaller")]
public class GameSystemInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {

        Container
            .Bind<FlipController>()
            .AsSingle();

    }
}

