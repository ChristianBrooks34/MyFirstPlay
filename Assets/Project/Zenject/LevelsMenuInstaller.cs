using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelsMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _exitLevelMenuButton;
    [Header("LevelOpenButton")]
    [SerializeField] private GameObject _levelOpenButtonPrefab;
    [SerializeField] private Transform _parentForLevelOpenButton;

    public override void InstallBindings()
    {
        BindUI();

        Container
            .BindInterfacesTo<LevelsMenu>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<SpawnerLevelButton>()
            .AsSingle()
            .WithArguments(_levelOpenButtonPrefab, _parentForLevelOpenButton);

        Container
            .BindInterfacesTo<ExitLevelMenuButton>()
            .AsSingle()
            .WithArguments(_exitLevelMenuButton)
            .NonLazy();
    }

    private void BindUI()
    {

    }
}