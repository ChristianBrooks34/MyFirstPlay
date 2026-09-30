using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelProfile : BaseProfile<LevelData>
{
    [SerializeField] private LevelData _levelData;

    public override LevelData BaseData
    {
        get
        {
            return _levelData;
        }
        set
        {
            if (value == null) return;
            _levelData = value;
        }
    }
}

[Serializable]
public class LevelData : BaseData
{
    [NonSerialized] public PlayerProfile Player;

    public int NumberLevel;
    public int NumberMap;
    public LevelType LevelType;
    public LevelState LevelState;

    public Vector2 PointSpawnPlayer;
    public List<Wave> Waves = new List<Wave>();
}
