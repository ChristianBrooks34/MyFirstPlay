using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevelButton
{
    private readonly Game _game;
    private readonly GlobalEventManager _globalEventManager;
    private readonly Button _button;
    private readonly ResultLevelMenu _resultLevelMenu;

    public RestartLevelButton(Game game, GlobalEventManager globalEventManager, Button button, ResultLevelMenu resultLevelMenu)
    {
        _game = game;
        _resultLevelMenu = resultLevelMenu;
        _globalEventManager = globalEventManager;
        _button = button;

        _button.onClick.AddListener(OnButtonClicked);
        _resultLevelMenu.OnDisposable += Dispose;
    }

    public void OnButtonClicked()
    {
        SceneManager.LoadScene("Map" + _game.GameData.CurrentLevel.NumberMap);
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _resultLevelMenu.OnDisposable -= Dispose;
    }
}
