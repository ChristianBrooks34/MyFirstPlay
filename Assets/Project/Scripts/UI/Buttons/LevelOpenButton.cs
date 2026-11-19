using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LevelOpenButton : SoundButton
{
    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;
    private readonly LevelOpenButtonView _view;

    private LevelData _levelData;

    public LevelOpenButton(Game game, GlobalEventManager globalEventManager, SoundsEventManager soundsEventManager,
        LevelOpenButtonView view) : base(view.Button, soundsEventManager, ButtonType.OpenLevel)
    {
        _view = view;
        _game = game;
        _globalEventManager = globalEventManager;
    }

    public void Initialize(LevelData levelProfile)
    {
        _levelData = levelProfile;

        _view.LevelCompletedPanel.SetActive(levelProfile.LevelState == LevelState.Completed);
        _view.LevelClosedPanel.SetActive(levelProfile.LevelState == LevelState.Locked);
        _view.NumberLevel.text = levelProfile.NumberLevel.ToString();

        _view.NumberLevel.text = levelProfile.NumberLevel.ToString();

        base.Initialize();
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        if (_levelData.LevelState == LevelState.Completed
            //|| _levelData.LevelState == LevelState.Locked
            )
            return;

        _globalEventManager.TriggerOpenLevel(_levelData);
        _game.GameData.CurrentLevel = _levelData;
        SceneManager.LoadScene("Map" + _levelData.NumberMap);
    }

    public class Factory : PlaceholderFactory<LevelOpenButtonView, LevelOpenButton> { }
}
