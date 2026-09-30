using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ExitLevelButton : IInitializable, IDisposable
{
    private readonly Button _exitGameButton;
    private readonly GlobalEventManager _globalEventManager;
    public ExitLevelButton(Button exitGameButton, GlobalEventManager globalEventManager)
    {
        _exitGameButton = exitGameButton;
        _globalEventManager = globalEventManager;
    }

    public void Initialize()
    {
        _exitGameButton.onClick.AddListener(OnButtonClicked);
    }

    public void Dispose()
    {
        _exitGameButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        _globalEventManager.TriggerExitLevel();

        SceneManager.LoadScene("Lobby");
    }
}
