using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayDevelopButton : MonoBehaviour
{
    public Text Name;
    public Text CurrentDevelop;
    public Text Price;
    public Image NotEnoughMoneyPanel;

    private GlobalEventManager _globalEventManager;
    private DevelopButton _developButton;
    private Wallet _wallet;

    [Inject]
    public void Construct(GlobalEventManager globalEventManager, Wallet wallet)
    {
        _wallet = wallet;
        _globalEventManager = globalEventManager;

        _globalEventManager.OnCangeMoneyInWallet += DisplayNotEnoughMoneyPanel;
    }

    private void OnDestroy()
    {
        _globalEventManager.OnCangeMoneyInWallet -= DisplayNotEnoughMoneyPanel;
    }

    public void Display(DevelopButton developButton)
    {
        if (developButton == null)
        {
            return;
        }

        _developButton = developButton;

        DisplayNotEnoughMoneyPanel(_wallet.Money);
        Name.text = _developButton.DevelopItem.Name;
        CurrentDevelop.text = _developButton.DevelopItem.NumberDevelop.ToString();
        Price.text = _developButton.DevelopItem.Price.ToString();
    }

    private void DisplayNotEnoughMoneyPanel(int currentMoney)
    {
        if (NotEnoughMoneyPanel == null)
        {
            Debug.LogError("NotEnoughMoneyPanel == null");
        }

        if (currentMoney >= _developButton.DevelopItem.Price)
        {
            NotEnoughMoneyPanel.gameObject.SetActive(false);
        }
        else
        {
            NotEnoughMoneyPanel.gameObject.SetActive(true);
        }
    }
}
