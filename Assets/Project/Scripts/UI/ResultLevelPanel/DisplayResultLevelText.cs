using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayResultLevelText
{
    [Inject(Id = "winLevelText")] private string _winLevelText;
    [Inject(Id = "lussLevelText")] private string _lussLevelText;

    private readonly Text _text;
    private readonly ResultLevelMenu _resultLevelMenu;

    public DisplayResultLevelText(Text text, ResultLevelMenu resultLevelMenu)
    {
        _text = text;
        _resultLevelMenu = resultLevelMenu;

        _resultLevelMenu.OnOpen += OnOpenresultLevelMenu;
        _resultLevelMenu.OnDisposable += Dispose;
    }

    private void OnOpenresultLevelMenu(bool isWinLevel)
    {
        Display(isWinLevel);
    }

    public void Display(bool isWinLevel)
    {
        if (_text == null)
        {
            Debug.LogError($"text == null");
            return;
        }

        if (isWinLevel)
        {
            _text.text = _winLevelText;
        }
        else
        {
            _text.text = _lussLevelText;
        }
    }

    public void Dispose()
    {
        _resultLevelMenu.OnOpen -= OnOpenresultLevelMenu;
        _resultLevelMenu.OnDisposable -= Dispose;
    }
}

