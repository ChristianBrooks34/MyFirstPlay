using System;
using UnityEngine.UI;

public class OpenPlayersPanelButton : SoundButton
{
    private readonly PlayerHub _playerHub;
    private readonly GlobalEventManager _globalEventManager;

    public OpenPlayersPanelButton(Button openPlayersPanelButton, PlayerHub playerHub, GlobalEventManager globalEventManager,
        SoundsEventManager soundsEventManager) : base(openPlayersPanelButton, soundsEventManager)
    {
        _globalEventManager = globalEventManager;
        _playerHub = playerHub;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        _playerHub.Open();

        _globalEventManager.TriggerOpenPlayerPanel();
    }
}
