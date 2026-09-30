using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitInStartMenuButton : IDisposable
{
    private readonly Button _exitInMainMenuButton;

    public ExitInStartMenuButton(Button exitInMainMenuButton)
    {
        _exitInMainMenuButton = exitInMainMenuButton;

        _exitInMainMenuButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _exitInMainMenuButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
