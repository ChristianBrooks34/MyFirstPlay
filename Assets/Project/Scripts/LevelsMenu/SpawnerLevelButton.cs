using UnityEngine;
using Zenject;

public class SpawnerLevelButton
{
    private readonly DiContainer _diContainer;
    private readonly GameObject _levelOpenButtonPrefab;
    private readonly Transform _parentForlevelOpenButton;

    public SpawnerLevelButton(GameObject levelOpenButtonPrefab, DiContainer diContainer, Transform parentForlevelOpenButton)
    {
        _parentForlevelOpenButton = parentForlevelOpenButton;
        _levelOpenButtonPrefab = levelOpenButtonPrefab;
        _diContainer = diContainer;
    }

    public void Spawn(LevelData levelProfile)
    {
        var go = _diContainer.InstantiatePrefab(_levelOpenButtonPrefab, _parentForlevelOpenButton);

        var levelOpenButton = go.GetComponent<LevelOpenButton>();

        levelOpenButton.Initialize(levelProfile);
    }
}
