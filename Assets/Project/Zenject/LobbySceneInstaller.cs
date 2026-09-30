using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbySceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playersHub;
    [SerializeField] private GameObject _playerButtonPrefab;
    [SerializeField] private Transform _parentForPlayerButton;
    [SerializeField] private GameObject _playerDisplayButtonPrefab;
    [SerializeField] private Transform _parentDevelopButton;
    [SerializeField] private Transform _parentForPlayer;

    [Header("CurrentPlayerPanel")]
    [SerializeField] private Text _nameCurrentPlayer;
    [SerializeField] private Text _playerLevel;
    [SerializeField] private string _displayFormatPlayerLevel;

    [Header("Buttons")]
    [SerializeField] private Button _openLevelMenuButton;
    [SerializeField] private Button _openPlayersPanelButton;
    [SerializeField] private Button _exitPlayerHubButton;
    [SerializeField] private Button _exitInStartMenuButton;

    [Header("PlayerStats")]
    [SerializeField] private string _displayPlayerStatsTextFormat;
    [SerializeField] private GameObject _stateItemPrefab;
    [SerializeField] private Transform _parentForStateItem;

    public override void InstallBindings()
    {
        BindButtons();

        Container
            .BindInterfacesTo<PlayerStatsPanel>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<SpawnerPlayerStatsItem>()
            .AsSingle()
            .WithArguments(_stateItemPrefab, _parentForStateItem);

        Container
            .Bind<DisplayPlayerStatsItem>()
            .AsSingle()
            .WithArguments(_displayPlayerStatsTextFormat);

        Container
            .BindInterfacesTo<SpawnerDevelopButton>()
            .AsSingle()
            .WithArguments(_playerDisplayButtonPrefab, _parentDevelopButton)
            .NonLazy();

        Container
            .Bind<SpawnerPlayerButton>()
            .AsSingle()
            .WithArguments(_playerButtonPrefab, _parentForPlayerButton);

        Container
            .BindInterfacesTo<ExitInStartMenuButton>()
            .AsSingle()
            .WithArguments(_exitInStartMenuButton)
            .NonLazy();

        Container
            .Bind<PlayerHub>()
            .AsSingle()
            .WithArguments(_playersHub);

        Container
            .BindInterfacesTo<CurrentPlayerPanel>()
            .AsCached()
            .WithArguments(_nameCurrentPlayer, _parentForPlayer)
            .NonLazy();

        Container
            .BindInterfacesTo<DisplayPlayerLevel>()
            .AsSingle()
            .WithArguments(_playerLevel, _displayFormatPlayerLevel)
            .NonLazy();
    }

    private void BindButtons()
    {
        Container
            .BindInterfacesTo<OpenLevelMenuButton>()
            .AsSingle()
            .WithArguments(_openLevelMenuButton)
            .NonLazy();

        Container
            .BindInterfacesTo<OpenPlayersPanelButton>()
            .AsSingle()
            .WithArguments(_openPlayersPanelButton)
            .NonLazy();

        Container
            .BindInterfacesTo<ExitPlayerHubButton>()
            .AsSingle()
            .WithArguments(_exitPlayerHubButton)
            .NonLazy();
    }
}