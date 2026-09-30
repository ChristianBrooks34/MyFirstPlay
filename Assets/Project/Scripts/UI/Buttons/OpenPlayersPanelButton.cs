using System;
using UnityEngine.UI;

public class OpenPlayersPanelButton : IDisposable
{
    private readonly Button _openPlayersPanelButton;
    private readonly PlayerHub _playerHub;
    private readonly GlobalEventManager _globalEventManager;

    public OpenPlayersPanelButton(Button openPlayersPanelButton, PlayerHub playerHub, GlobalEventManager globalEventManager)
    {
        _openPlayersPanelButton = openPlayersPanelButton;
        _globalEventManager = globalEventManager;
        _playerHub = playerHub;

        _openPlayersPanelButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _openPlayersPanelButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        _playerHub.Open();

        _globalEventManager.TriggerOpenPlayerPanel();
    }
}
