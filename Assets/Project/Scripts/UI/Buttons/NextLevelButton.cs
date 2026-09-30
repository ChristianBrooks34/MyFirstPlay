using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NextLevelButton
{
    private readonly Button _button;

    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;
    private readonly ResultLevelMenu _resultLevelMenu;

    public NextLevelButton(Game game, GlobalEventManager globalEventManager, ResultLevelMenu resultLevelMenu, Button button)
    {
        _game = game;
        _button = button;
        _globalEventManager = globalEventManager;
        _resultLevelMenu = resultLevelMenu;

        _button.onClick.AddListener(OnButtonClicked);
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

    public void OnButtonClicked()
    {
        var nextLevleData = _game.GameData.SerializedAllLevels
            .Where(x => x.NumberLevel == _game.GameData.CurrentLevel.NumberLevel + 1)
            .FirstOrDefault();

        _globalEventManager.TriggerOpenLevel(nextLevleData);
        _game.GameData.CurrentLevel = nextLevleData;

        SceneManager.LoadScene("Map" + nextLevleData.NumberMap);
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _resultLevelMenu.OnClouse -= Dispose;
        _resultLevelMenu.OnOpen -= OnOpenResultLevelMenu;
    }
}

