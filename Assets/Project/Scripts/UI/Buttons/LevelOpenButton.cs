using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LevelOpenButton : MonoBehaviour, IDisposable
{
    public Button Button;
    public Text NumberLevel;
    public GameObject LevelCompletedPanel;
    public GameObject LevelClosedPanel;

    private Game _game;
    private LevelData _levelData;
    private GlobalEventManager _globalEventManager;

    [Inject]
    public void Construct(Game game, GlobalEventManager globalEventManager)
    {
        _game = game;
        _globalEventManager = globalEventManager;
    }

    public void Initialize(LevelData levelProfile)
    {
        _levelData = levelProfile;

        switch (levelProfile.LevelState)
        {
            case LevelState.Available:
                LevelCompletedPanel.SetActive(false);
                LevelClosedPanel.SetActive(false);
                break;

            case LevelState.Locked:
                LevelClosedPanel.SetActive(true);
                LevelCompletedPanel.SetActive(false);
                break;

            case LevelState.Completed:
                LevelCompletedPanel.SetActive(true);
                LevelClosedPanel.SetActive(false);
                break;
        }

        NumberLevel.text = levelProfile.NumberLevel.ToString();

        Button.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        if (_levelData.LevelState == LevelState.Completed
            //|| _levelData.LevelState == LevelState.Locked
            )
            return;

        _globalEventManager.TriggerOpenLevel(_levelData);
        _game.GameData.CurrentLevel = _levelData;
        SceneManager.LoadScene("Map" + _levelData.NumberMap);
    }

    public void Dispose()
    {
        Button.onClick.RemoveListener(OnButtonClicked);
    }
}
