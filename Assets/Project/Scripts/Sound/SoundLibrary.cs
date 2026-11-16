using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Sound/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    [Serializable]
    public class SoundEntry
    {
        public ButtonType type;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<SoundEntry> sounds = new();

    public bool TryGetSound(ButtonType type, out AudioClip clip, out float volume)
    {
        clip = null;
        volume = 1f;

        foreach (var entry in sounds)
        {
            if (entry.type != type) continue;
            clip = entry.clip;
            volume = entry.volume;
            return clip != null;
        }
        return false;
    }
}
