using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenLevelMenuButton : SoundButton
{
    private readonly Button _openLevelMenuButton;

    public OpenLevelMenuButton(Button exitGameButton, SoundsEventManager soundsEventManager)
        : base(exitGameButton, soundsEventManager)
    {
        _openLevelMenuButton = exitGameButton;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        SceneManager.LoadScene("LevelsMenu");
    }
}
