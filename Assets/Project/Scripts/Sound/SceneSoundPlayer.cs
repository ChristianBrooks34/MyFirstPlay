using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneSoundPlayer : IInitializable, IDisposable
{
    private readonly SceneMusicConfig _sceneMusicConfig;
    private readonly GlobalEventManager _globalEventManager;
    private readonly AudioSource _audioSource;
    private readonly SoundManager _soundManager;

    public SceneSoundPlayer(SceneMusicConfig sceneMusicConfig, GlobalEventManager globalEventManager, AudioSource audioSource,
        SoundManager soundManager)
    {
        _sceneMusicConfig = sceneMusicConfig;
        _globalEventManager = globalEventManager;
        _audioSource = audioSource;
        _soundManager = soundManager;
    }

    public void Initialize()
    {
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;

        _globalEventManager.OnChangeScene += PlaySound;
        PlaySound(SceneType.MainMenu);
    }

    private void PlaySound(SceneType sceneType)
    {
        var musicByType = _sceneMusicConfig.GetMusingBuType(sceneType);

        if (musicByType.Clip == null || (_audioSource.clip == musicByType.Clip && _audioSource.isPlaying))
            return;

        _soundManager.Play(musicByType.Clip, _audioSource, musicByType.Value, true);
    }

    public void Dispose()
    {
        _globalEventManager.OnChangeScene -= PlaySound;
    }
}
