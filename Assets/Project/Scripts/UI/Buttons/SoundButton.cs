using System;
using UnityEngine.UI;
using Zenject;

public abstract class SoundButton : IInitializable, IDisposable
{
    private readonly Button _button;
    private readonly SoundsEventManager _soundsEventManager;

    protected SoundButton(Button button, SoundsEventManager soundsEventManager)
    {
        _button = button;
        _soundsEventManager = soundsEventManager;
    }

    public void Initialize() => _button.onClick.AddListener(HandleClick);
    public void Dispose() => _button.onClick.RemoveListener(HandleClick);

    private void HandleClick()
    {
        OnButtonClicked();
    }

    protected abstract void OnButtonClicked();
}
