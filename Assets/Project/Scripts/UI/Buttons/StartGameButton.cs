using System;
using UnityEngine.UI;
using Zenject;

public class StartGameButton : SoundButton, IInitializable, IDisposable
{
    private readonly GameLauncer _gameLauncer;
    private readonly GlobalEventManager _globalEventManager;

    public StartGameButton(Button startGameButton, GameLauncer gameLauncer, GlobalEventManager globalEventManager,
        SoundsEventManager soundsEventManager) : base(startGameButton, soundsEventManager)
    {
        _gameLauncer = gameLauncer;
        _globalEventManager = globalEventManager;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        _globalEventManager.TriggerStartGame();

        _gameLauncer.StartPlay();
    }
}
