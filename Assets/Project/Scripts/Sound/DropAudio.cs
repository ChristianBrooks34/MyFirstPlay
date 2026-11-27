
using System;
using UnityEngine;

public class DropAudio
{
    private readonly SoundManager _soundManager;

    private AudioSource _audioSource;

    public DropAudio(SoundManager soundManager)
    {
        _soundManager = soundManager;

        CreateAudioSource();
    }

    public void PlayPickableAudio(DropProfile profile)
    {
        if (_audioSource == null) CreateAudioSource();
        _soundManager.Play(profile.BaseData.PickubSound.Clip, _audioSource, profile.BaseData.PickubSound.Value);
    }

    private void CreateAudioSource()
    {
        _audioSource = new GameObject("DropAudioSourse").AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }
}

