using System;
using Zenject;

public class PlayerButton : SoundButton
{
    private readonly Game _game;
    private readonly PlayerButtonView _playerButtonView;
    private readonly GlobalEventManager _globalEventManager;
    private readonly SpawnUnitForUIFactory _spawnUnitForUIFactory;
    public PlayerProfile PlayerProfile { get; set; }

    public PlayerButton(Game game, GlobalEventManager globalEventManager, SpawnUnitForUIFactory spawnUnitForUIFactory,
        PlayerButtonView playerButtonView, SoundsEventManager soundsEventManager)
        : base(playerButtonView.Button, soundsEventManager)
    {
        _game = game;
        _globalEventManager = globalEventManager;
        _spawnUnitForUIFactory = spawnUnitForUIFactory;
        _playerButtonView = playerButtonView;
    }

    public void Initialize(PlayerProfile playerProfile, PlayerData playerData)
    {
        PlayerProfile = playerProfile;

        PlayerProfile.Data = playerData;

        _playerButtonView.Name.text = playerProfile.Data.Name;

        _spawnUnitForUIFactory.PlayerEmptySpawn(PlayerProfile, _playerButtonView.PlayerArea.transform, false);

        _playerButtonView.Button.onClick.AddListener(OnButtonClicked);
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        if (_game.GameData.SelectedPlayer.Value != PlayerProfile)
        {
            _game.GameData.SelectedPlayer.Value = PlayerProfile;

            _globalEventManager.TriggerChangeSelectedPlayer(PlayerProfile);
        }
    }

    public class Factory : PlaceholderFactory<PlayerButtonView, PlayerButton> { }

}
