using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenLevelMenuButton : IDisposable
{
    private readonly Button _openLevelMenuButton;

    public OpenLevelMenuButton(Button exitGameButton)
    {
        _openLevelMenuButton = exitGameButton;

        _openLevelMenuButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _openLevelMenuButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene("LevelsMenu");
    }
}
