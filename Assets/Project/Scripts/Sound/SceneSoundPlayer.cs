using System;
using UnityEngine;

public class SceneSoundPlayer : IDisposable
{
    private readonly SceneMusicConfig _sceneMusicConfig;
    private readonly GlobalEventManager _globalEventManager;
    private readonly AudioSource _audioSource;

    public SceneSoundPlayer(SceneMusicConfig sceneMusicConfig, GlobalEventManager globalEventManager, AudioSource audioSource)
    {
        _sceneMusicConfig = sceneMusicConfig;
        _globalEventManager = globalEventManager;
        _audioSource = audioSource;

        _audioSource.loop = true;
        _globalEventManager.OnChangeScene += PlaySound;
    }

    private void PlaySound(SceneType sceneType)
    {
        var clip = _sceneMusicConfig.GetClip(sceneType);

        if (clip == null || (_audioSource.clip == clip && _audioSource.isPlaying))
            return;

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Dispose()
    {
        _globalEventManager.OnChangeScene -= PlaySound;
    }
}

