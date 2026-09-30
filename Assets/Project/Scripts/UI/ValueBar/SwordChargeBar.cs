using System.Linq;
using UnityEngine;
using Zenject;

public class SwordChargeBar : ValueBar
{
    [SerializeField] private GameObject _bar;

    private Player _player;
    private DisplayValueBar _displayValueBar;
    private PlayerChargeAttack _playerChargeAttack;

    [Inject]
    public void Construct(DisplayValueBar displayValueBar)
    {
        _displayValueBar = displayValueBar;
    }

    public void Initialize(Player player)
    {
        _player = player;

        _playerChargeAttack = _player.BaseAttacks
            .Where((x) => x is PlayerChargeAttack)
            .Cast<PlayerChargeAttack>()
            .FirstOrDefault();

        if (_playerChargeAttack != null)
        {
            _playerChargeAttack.OnStartedCharge += OnStartHold;
            _playerChargeAttack.OnCanceledCharge += OnCancelHold;

            _playerChargeAttack.OnChangeCharge += HealthChangedHandler;

            HealthChangedHandler(0);
        }
        else
        {
            Debug.LogError("_playerChargeAttack == null");
        }
    }

    private void HealthChangedHandler(float currentCharge)
    {
        _displayValueBar.Display(currentCharge, _playerChargeAttack.MaxHoldTime);
    }

    private void OnStartHold()
    {
        _bar.SetActive(true);
    }

    private void OnCancelHold()
    {
        _bar.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_playerChargeAttack != null)
        {
            _playerChargeAttack.OnStartedCharge -= OnStartHold;
            _playerChargeAttack.OnCanceledCharge -= OnCancelHold;

            _playerChargeAttack.OnChangeCharge -= HealthChangedHandler;
        }
    }
}
