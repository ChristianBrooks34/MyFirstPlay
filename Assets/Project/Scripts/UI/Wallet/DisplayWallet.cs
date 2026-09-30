using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayWallet : MonoBehaviour, IDisposable
{
    [SerializeField] private string _displayFormatMoney;
    [SerializeField] private Text _money;

    private GlobalEventManager _globalEventManager;
    private Wallet _wallet;

    [Inject]
    public void Construct(GlobalEventManager globalEventManager, Wallet wallet)
    {
        _globalEventManager = globalEventManager;
        _wallet = wallet;

        _globalEventManager.OnCangeMoneyInWallet += Display;

        Display(_wallet.Money);
    }

    public void Display(int currentMoney)
    {
        _money.text = string.Format(_displayFormatMoney, currentMoney);
    }

    public void Dispose()
    {
        _globalEventManager.OnCangeMoneyInWallet -= Display;
    }

    private void OnDestroy()
    {
        Dispose();
    }
}
