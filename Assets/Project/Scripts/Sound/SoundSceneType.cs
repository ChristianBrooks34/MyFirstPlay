using System;
using UnityEngine;

[Serializable]
public struct SoundSceneType
{
    public SceneType Type;
    public AudioClip Clip;
    [Range(0f, 1f)]  public float Value;
}

