using System;
using UnityEngine.UI;
using Zenject;

public class StartGameButton : IInitializable, IDisposable
{
    private readonly Button _startGameButton;
    private readonly GameLauncer _gameLauncer;
    private readonly GlobalEventManager _globalEventManager;

    public StartGameButton(Button startGameButton, GameLauncer gameLauncer, GlobalEventManager globalEventManager)
    {
        _startGameButton = startGameButton;
        _gameLauncer = gameLauncer;
        _globalEventManager = globalEventManager;
    }

    public void Initialize()
    {
        _startGameButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _startGameButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        _globalEventManager.TriggerStartGame();

        _gameLauncer.StartPlay();
    }
}
