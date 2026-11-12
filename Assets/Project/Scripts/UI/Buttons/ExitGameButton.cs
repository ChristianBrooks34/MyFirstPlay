using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class ExitGameButton : SoundButton
{
    private readonly ApplicationFinisher _applicationFinisher;
    private readonly GlobalEventManager _globalEventManager;

    public ExitGameButton(Button exitGameButton, ApplicationFinisher applicationFinisher, GlobalEventManager globalEventManager, 
        SoundsEventManager soundsEventManager) : base (exitGameButton, soundsEventManager)
    {
        _globalEventManager = globalEventManager;
        _applicationFinisher = applicationFinisher;
    }

    protected override void OnButtonClicked()
    {
        _globalEventManager.TriggerExitGame();
        _applicationFinisher.Finish();
    }
}
