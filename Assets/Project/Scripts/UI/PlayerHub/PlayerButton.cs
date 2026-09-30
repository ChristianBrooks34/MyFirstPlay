using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerButton : MonoBehaviour, IDisposable
{
    public Button Button;
    public Text Name;
    public Image Icon;
    [SerializeField] private GameObject _playerArea;

    private Game _game;
    private GlobalEventManager _globalEventManager;
    private SpawnUnitForUIFactory _spawnUnitForUIFactory;
    public PlayerProfile PlayerProfile { get; set; }

    [Inject]
    public void Construct(Game game, GlobalEventManager globalEventManager, SpawnUnitForUIFactory spawnUnitForUIFactory)
    {
        _game = game;
        _globalEventManager = globalEventManager;
        _spawnUnitForUIFactory = spawnUnitForUIFactory;
    }

    public void Initialize(PlayerProfile playerProfile, PlayerData playerData)
    {
        PlayerProfile = playerProfile;

        PlayerProfile.Data = playerData;

        Name.text = playerProfile.Data.Name;

        _spawnUnitForUIFactory.PlayerEmptySpawn(PlayerProfile, _playerArea.transform, false);

        Button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        if (_game.GameData.SelectedPlayer.Value != PlayerProfile)
        {
            _game.GameData.SelectedPlayer.Value = PlayerProfile;

            _globalEventManager.TriggerChangeSelectedPlayer(PlayerProfile);
        }
    }

    public void Dispose()
    {
        Button.onClick.RemoveListener(OnButtonClicked);
    }
}
