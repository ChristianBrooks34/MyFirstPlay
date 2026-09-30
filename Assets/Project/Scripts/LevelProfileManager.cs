public class LevelProfileManager
{
    private readonly Game _game;

    public LevelProfileManager(Game game)
    {
        _game = game;
    }

    public void InitializeAllLevelProfile()
    {
        var levelsProfile = _game.GameData.AllLevels;

        foreach (var level in levelsProfile)
        {
            level.BaseData.LevelState = LevelState.Locked;
        }
    }
}
