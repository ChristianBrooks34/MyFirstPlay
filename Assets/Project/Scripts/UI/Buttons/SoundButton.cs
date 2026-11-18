using System;
using UnityEngine.UI;
using Zenject;

public abstract class SoundButton : IInitializable, IDisposable
{
    private readonly Button _button;
    private readonly SoundsEventManager _soundsEventManager;
    private readonly ButtonType _buttonType;

    protected SoundButton(Button button, SoundsEventManager soundsEventManager, ButtonType buttonType = ButtonType.Default)
    {
        _button = button;
        _soundsEventManager = soundsEventManager;
        _buttonType = buttonType;
    }

    public void Initialize() => _button.onClick.AddListener(OnButtonClicked);
    public void Dispose() => _button.onClick.RemoveListener(OnButtonClicked);

    protected virtual void OnButtonClicked()
    {
        _soundsEventManager.TriggerButtonClicked(_buttonType);
    }
}
