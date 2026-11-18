using UnityEngine;
using Zenject;

public class SpawnerLevelButton
{
    private readonly IInstantiator _iInstantiator;
    private readonly GameObject _levelOpenButtonPrefab;
    private readonly Transform _parentForlevelOpenButton;
    private readonly LevelOpenButton.Factory _levelOpenButtonFactory;

    public SpawnerLevelButton(GameObject levelOpenButtonPrefab, IInstantiator iInstantiator, Transform parentForlevelOpenButton,
        LevelOpenButton.Factory levelOpenButtonFactory)
    {
        _parentForlevelOpenButton = parentForlevelOpenButton;
        _levelOpenButtonFactory = levelOpenButtonFactory;
        _levelOpenButtonPrefab = levelOpenButtonPrefab;
        _iInstantiator = iInstantiator;
    }

    public void Spawn(LevelData levelProfile)
    {
        var go = _iInstantiator.InstantiatePrefab(_levelOpenButtonPrefab, _parentForlevelOpenButton);
        var view = go.GetComponent<LevelOpenButtonView>();

        var levelOpenButton = _levelOpenButtonFactory.Create(view);
        levelOpenButton.Initialize(levelProfile);
    }
}
