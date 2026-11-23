using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseUnitAudio", menuName = "Sound/Unit Audio Config")]
public class UnitAudioConfig : ScriptableObject
{
    [Header("Base Sounds")]
    public AudioClip[] Footsteps;
    public AudioClip[] TakeDamage;
    public AudioClip[] Death;

    [Header("Settings")]
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 0.3f)] public float PitchRandomness = 0.05f;

    public AudioClip GetRandomFootstep() => GetRandomClip(Footsteps);
    public AudioClip GetRandomTakeDamage() => GetRandomClip(TakeDamage);
    public AudioClip GetRandomDeath() => GetRandomClip(Death);

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
