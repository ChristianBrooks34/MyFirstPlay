using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitInStartMenuButton : SoundButton
{
    public ExitInStartMenuButton(Button exitInMainMenuButton, SoundsEventManager soundsEventManager)
        : base(exitInMainMenuButton, soundsEventManager)
    {

    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();

        SceneManager.LoadScene("StartMenu");
    }
}
