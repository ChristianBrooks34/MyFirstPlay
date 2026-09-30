using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelUIInstaller : MonoInstaller
{
    [SerializeField] private Button _exitInMenuButton;
    [SerializeField] private PlayerLevelBar _levelProgressBar;
    [SerializeField] private Image _imageDisplayValueLevelProgressBar;
    [SerializeField] private Text _displayText;
    [SerializeField] private string _formatDisplayPlayerLevelBar;

    [Header("ResultLevelMenu")]
    [SerializeField] private Text _resultLevelText;
    [SerializeField] private string _winLevelText;
    [SerializeField] private string _lussLevelText;
    [SerializeField] private GameObject _playerLevelMenu;
    [SerializeField] private Button _restartLevelButton;
    [SerializeField] private Button _exitLevelMenuButton;
    [SerializeField] private Button _nextLevelButton;

    public override void InstallBindings()
    {
        Container
            .BindInterfacesTo<ExitLevelButton>()
            .AsSingle()
            .WithArguments(_exitInMenuButton)
            .NonLazy();

        BindResultLevelMenu();

        BindLevelProgressBar();
    }

    private void BindLevelProgressBar()
    {
        Container
            .BindInterfacesTo<PlayerLevelBar>()
            .FromInstance(_levelProgressBar)
            .AsSingle()
            .NonLazy();

        Container
            .Bind<Text>()
            .FromInstance(_displayText)
            .AsSingle();

        Container
            .Bind<DisplayPlayerLevelBar>()
            .AsCached()
            .WithArguments(_levelProgressBar, _imageDisplayValueLevelProgressBar, _formatDisplayPlayerLevelBar);
    }

    private void BindResultLevelMenu()
    {
        Container
            .Bind<RestartLevelButton>()
            .AsSingle()
            .WithArguments(_restartLevelButton)
            .NonLazy();

        Container
            .Bind<ExitLevelMenuButton>()
            .AsSingle()
            .WithArguments(_exitLevelMenuButton)
            .NonLazy();

        Container
            .Bind<NextLevelButton>()
            .AsSingle()
            .WithArguments(_nextLevelButton)
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<ResultLevelMenu>()
            .AsSingle()
            .WithArguments(_playerLevelMenu)
            .NonLazy();

        Container
            .Bind<DisplayResultLevelText>()
            .AsSingle()
            .WithArguments(_resultLevelText)
            .NonLazy();

        Container
            .Bind<string>()
            .WithId("winLevelText")
            .FromInstance(_winLevelText)
            .AsCached();

        Container
            .Bind<string>()
            .WithId("lussLevelText")
            .FromInstance(_lussLevelText)
            .AsCached();
    }
}

