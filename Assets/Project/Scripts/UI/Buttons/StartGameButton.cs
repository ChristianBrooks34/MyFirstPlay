using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartGameButton : SoundButton
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
        Debug.LogError("StartGameButton");
        _globalEventManager.TriggerStartGame();

        _gameLauncer.StartPlay();
    }
}
