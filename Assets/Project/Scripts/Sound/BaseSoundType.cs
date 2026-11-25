using System;
using UnityEngine;

[Serializable]

public class BaseSoundType
{
    public AudioClip Clip;
    [Range(0f, 1f)] public float Value;
}

