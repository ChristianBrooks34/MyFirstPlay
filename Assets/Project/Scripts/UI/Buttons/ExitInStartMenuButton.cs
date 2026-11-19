using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitInStartMenuButton : SoundButton
{
    private readonly GlobalEventManager _eventManager;
    public ExitInStartMenuButton(Button exitInMainMenuButton, SoundsEventManager soundsEventManager, GlobalEventManager eventManager)
        : base(exitInMainMenuButton, soundsEventManager)
    {
        _eventManager = eventManager;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        SceneManager.LoadScene("StartMenu");
        _eventManager.TriggerChangeScene(SceneType.MainMenu);
    }
}
