using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _exitGameButton;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesTo<StartGameButton>()
            .AsSingle()
            .WithArguments(_startGameButton)
            .NonLazy();

        Container
            .BindInterfacesTo<ExitGameButton>()
            .AsSingle()
            .WithArguments(_exitGameButton)
            .NonLazy();
    }
}
