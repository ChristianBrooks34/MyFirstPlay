using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ExitLevelButton : SoundButton
{
    private readonly GlobalEventManager _globalEventManager;
    public ExitLevelButton(Button exitGameButton, GlobalEventManager globalEventManager, SoundsEventManager soundsEventManager)
        : base(exitGameButton, soundsEventManager)
    {
        _globalEventManager = globalEventManager;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        _globalEventManager.TriggerExitLevel();

        SceneManager.LoadScene("Lobby");
    }
}
