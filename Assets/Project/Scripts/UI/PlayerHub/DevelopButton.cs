using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DevelopButton : MonoBehaviour
{
    public Button Button;
    public DisplayDevelopButton _displayDevelopButton;

    private Wallet _wallet;
    private GlobalEventManager _globalEventManager;

    public DevelopItemFloat DevelopItem { get; private set; }

    [Inject]
    public void Construct(GlobalEventManager globalEventManager, Wallet wallet)
    {
        _wallet = wallet;
        _globalEventManager = globalEventManager;

        _globalEventManager.OnExitGame += OnExitGame;
    }

    public void Initialize(DevelopItemFloat developItemFloat)
    {
        DevelopItem = developItemFloat;

        _displayDevelopButton.Display(this);

        Button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        Develop();
    }

    private void Develop()
    {
        if (_wallet.Money >= DevelopItem.Price)
        {
            _wallet.Substract(DevelopItem.Price);

            DevelopItem.Develop();

            _globalEventManager.TriggerDevelopPlayer();

            _displayDevelopButton.Display(this);
        }
    }

    public void Dispose()
    {
        Button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnExitGame()
    {
        DevelopItem.NumberDevelop = 0;
        DevelopItem.Price = DevelopItem.StartPrice;
        DevelopItem.CurrentValue = DevelopItem.StartValue;
    }
}
