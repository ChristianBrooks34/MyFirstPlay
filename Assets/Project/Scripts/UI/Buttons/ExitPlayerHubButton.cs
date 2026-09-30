using System;
using UnityEngine.UI;

public class ExitPlayerHubButton : IDisposable
{
    private readonly Button _exitInLobbyButton;
    private readonly PlayerHub _playerHub;

    public ExitPlayerHubButton(Button exitInLobbyButton, PlayerHub playerHub)
    {
        _exitInLobbyButton = exitInLobbyButton;

        _exitInLobbyButton.onClick.AddListener(OnButtonClicked);
        _playerHub = playerHub;
    }

    public void Dispose()
    {
        _exitInLobbyButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        _playerHub.Clouse();
    }
}
