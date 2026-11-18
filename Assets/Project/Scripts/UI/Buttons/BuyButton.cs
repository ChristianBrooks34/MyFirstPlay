using UnityEngine.UI;

public class BuyButton : SoundButton
{

    public BuyButton(Button button, SoundsEventManager soundsEventManager)
        : base(button, soundsEventManager, ButtonType.Buy)
    {

    }

    protected override void OnButtonClicked()
    {

    }
}
