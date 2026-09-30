using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitLevelMenuButton : IDisposable
{
    private readonly Button _exitLevelMenuButton;

    public ExitLevelMenuButton(Button exitLevelMenuButton)
    {
        _exitLevelMenuButton = exitLevelMenuButton;

        _exitLevelMenuButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _exitLevelMenuButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene("Lobby");
    }
}
