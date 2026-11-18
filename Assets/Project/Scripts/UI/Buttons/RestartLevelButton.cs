using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevelButton : SoundButton
{
    private readonly Game _game;
    private readonly ResultLevelMenu _resultLevelMenu;

    public RestartLevelButton(Game game, GlobalEventManager globalEventManager, Button button, ResultLevelMenu resultLevelMenu,
        SoundsEventManager soundsEventManager) : base(button, soundsEventManager)
    {
        _game = game;
        _resultLevelMenu = resultLevelMenu;

        _resultLevelMenu.OnDisposable += Dispose;
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        SceneManager.LoadScene("Map" + _game.GameData.CurrentLevel.NumberMap);
    }

    public new void Dispose()
    {
        base.Dispose();

        _resultLevelMenu.OnDisposable -= Dispose;
    }
}
