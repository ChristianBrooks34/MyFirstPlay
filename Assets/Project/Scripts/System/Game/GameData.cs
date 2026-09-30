using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class GameData
{
    public LevelData CurrentLevel;

    [NonSerialized] public ReactiveProperty<PlayerProfile> SelectedPlayer = new ReactiveProperty<PlayerProfile>();
    [NonSerialized] public List<PlayerProfile> AllPlayers = new List<PlayerProfile>();
    [NonSerialized] public List<LevelProfile> AllLevels = new List<LevelProfile>();

    // Поля для сериализации
    public LevelData SerializedCurrentLevel;

    [SerializeField] private PlayerData _serializedSelectedPlayer;
    public PlayerData SerializedSelectedPlayer
    { 
        get
        {
            if (_serializedSelectedPlayer != null) Debug.LogWarning($"{Time.frameCount} GameData Get; SerializedSelectedPlayer = {_serializedSelectedPlayer.Name}");
            return _serializedSelectedPlayer;
        }
        set
        {
            Debug.LogWarning($"{Time.frameCount}  1 GameData Set; value = {value}");

            if (value != null)
            {
                _serializedSelectedPlayer = value;
            }
        }
    }
    public List<PlayerData> SerializedAllPlayers = new List<PlayerData>();
    public List<LevelData> SerializedAllLevels = new List<LevelData>();
}
