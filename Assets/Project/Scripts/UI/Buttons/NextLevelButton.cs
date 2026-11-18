using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NextLevelButton : SoundButton
{
    private readonly Button _button;
    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;
    private readonly ResultLevelMenu _resultLevelMenu;

    public NextLevelButton(Game game, GlobalEventManager globalEventManager, ResultLevelMenu resultLevelMenu, Button button,
        SoundsEventManager soundsEventManager) : base(button, soundsEventManager)
    {
        _game = game;
        _button = button;
        _globalEventManager = globalEventManager;
        _resultLevelMenu = resultLevelMenu;

        Debug.LogError("NextLevelButton");

        _resultLevelMenu.OnClouse += Dispose;
        _resultLevelMenu.OnOpen += OnOpenResultLevelMenu;
    }

    private void OnOpenResultLevelMenu(bool isLevelWin)
    {
        if (!isLevelWin)
        {
            _button.gameObject.SetActive(false);
        }
    }

    protected override void OnButtonClicked()
    {
        Debug.LogError("NextLevelButton OnButtonClicked");
        base.OnButtonClicked();

        var nextLevleData = _game.GameData.SerializedAllLevels
            .Where(x => x.NumberLevel == _game.GameData.CurrentLevel.NumberLevel + 1)
            .FirstOrDefault();

        _globalEventManager.TriggerOpenLevel(nextLevleData);
        _game.GameData.CurrentLevel = nextLevleData;

        SceneManager.LoadScene("Map" + nextLevleData.NumberMap);
    }

    public new void Dispose()
    {
        base.Dispose();
        _resultLevelMenu.OnClouse -= Dispose;
        _resultLevelMenu.OnOpen -= OnOpenResultLevelMenu;
    }
}

