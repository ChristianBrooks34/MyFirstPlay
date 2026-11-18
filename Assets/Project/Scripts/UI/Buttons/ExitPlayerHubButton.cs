using System;
using UnityEngine.UI;

public class ExitPlayerHubButton : SoundButton
{
    private readonly PlayerHub _playerHub;

    public ExitPlayerHubButton(Button exitInLobbyButton, PlayerHub playerHub, SoundsEventManager soundsEventManager) 
        : base(exitInLobbyButton, soundsEventManager)
    {
        _playerHub = playerHub;
    }


    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        _playerHub.Clouse();
    }
}
