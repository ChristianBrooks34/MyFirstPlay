using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitLevelMenuButton : SoundButton
{
    public ExitLevelMenuButton(Button exitLevelMenuButton, SoundsEventManager soundsEventManager)
        : base(exitLevelMenuButton, soundsEventManager)
    {

    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        SceneManager.LoadScene("Lobby");
    }
}
