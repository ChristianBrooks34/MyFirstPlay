using UnityEngine;

public class Wallet
{
    private GlobalEventManager _globalEventManager;
    public int Money { get; private set; } = 100000;

    public Wallet(GlobalEventManager globalEventManager)
    {
        _globalEventManager = globalEventManager;
    }

    public Wallet(int money)
    {
        Money = money;
    }

    public void Add(int value)
    {
        if (value < 0)
        {
            Debug.LogError("Параметр value не может быть меньше нуля");
            return;
        }
        Money += value;

        if (_globalEventManager != null) _globalEventManager.TriggerCangeMoneyInWallet(Money);
    }

    public void Substract(int value)
    {
        if (value < 0)
        {
            Debug.LogError("Параметр value не может быть меньше нуля");
            return;
        }
        if (Money - value < 0) return;

        Money -= value;

        if (_globalEventManager != null) _globalEventManager.TriggerCangeMoneyInWallet(Money);
    }
}
