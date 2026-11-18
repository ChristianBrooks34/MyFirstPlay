using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DevelopButton : BuyButton
{
    private DisplayDevelopButton _displayDevelopButton;
    private SoundsEventManager _soundsEventManager;
    private Wallet _wallet;
    private GlobalEventManager _globalEventManager;
    public DevelopItemFloat DevelopItem { get; private set; }

    public DevelopButton(Button button, SoundsEventManager soundsEventManager, DisplayDevelopButton displayDevelopButton)
        : base(button, soundsEventManager)
    {
        _soundsEventManager = soundsEventManager;
        _displayDevelopButton = displayDevelopButton;
    }


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

        base.Initialize();
    }

    protected override void OnButtonClicked()
    {
        Develop();
    }

    private void Develop()
    {
        if (_wallet.Money >= DevelopItem.Price)
        {
            _soundsEventManager.TriggerButtonClicked(ButtonType.Buy);
            _wallet.Substract(DevelopItem.Price);

            DevelopItem.Develop();

            _globalEventManager.TriggerDevelopPlayer();

            _displayDevelopButton.Display(this);
        }
        else
        {
            _soundsEventManager.TriggerButtonClicked(ButtonType.BuyFail);
        }
    }

    private void OnExitGame()
    {
        DevelopItem.NumberDevelop = 0;
        DevelopItem.Price = DevelopItem.StartPrice;
        DevelopItem.CurrentValue = DevelopItem.StartValue;
    }

    public class Factory : PlaceholderFactory<Button, DisplayDevelopButton, DevelopButton> { }
}
