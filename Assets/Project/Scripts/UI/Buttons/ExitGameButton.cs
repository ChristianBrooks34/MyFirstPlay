using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExitGameButton : IInitializable, IDisposable
{
    private readonly Button _exitGameButton;
    private readonly ApplicationFinisher _applicationFinisher;
    private readonly GlobalEventManager _globalEventManager;

    public ExitGameButton(Button exitGameButton, ApplicationFinisher applicationFinisher, GlobalEventManager globalEventManager)
    {
        _exitGameButton = exitGameButton;
        _globalEventManager = globalEventManager;
        _applicationFinisher = applicationFinisher;
    }

    public void Initialize()
    {
        _exitGameButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _exitGameButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        _globalEventManager.TriggerExitGame();
        _applicationFinisher.Finish();
    }
}
