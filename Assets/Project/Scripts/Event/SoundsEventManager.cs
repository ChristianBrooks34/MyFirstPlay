using System;

public class SoundsEventManager
{
    public event Action<ButtonType> OnButtonClicked;

    public void TriggerButtonClicked(ButtonType buttonType)
    {
        OnButtonClicked?.Invoke(buttonType);
    }
}

