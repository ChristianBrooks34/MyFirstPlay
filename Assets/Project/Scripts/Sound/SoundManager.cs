using System;
using UnityEngine;
using Zenject;

public class SoundManager : IInitializable, IDisposable
{
    private readonly SoundsEventManager _eventManager;
    private readonly SoundLibrary _library;
    private readonly AudioVolumeFader _volumeFader;

    private AudioSource _audioSource;
    private bool _isInitialize;
    public SoundManager(SoundsEventManager eventManager, SoundLibrary library, AudioVolumeFader volumeFader)
    {
        _eventManager = eventManager;
        _library = library;
        _volumeFader = volumeFader;
    }

    public void Initialize()
    {
        _audioSource = new GameObject("SoundManagerAudioSource").AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        GameObject.DontDestroyOnLoad(_audioSource);

        _eventManager.OnButtonClicked += Play;

        _isInitialize = true;
    }

    public void Dispose()
    {
        _eventManager.OnButtonClicked -= Play;

        _isInitialize = false;
    }

    public void Play(ButtonType type, AudioSource customSource)
    {
        if (!_isInitialize || customSource == null) return;

        if (!_library.TryGetSound(type, out var clip, out var volume) &&
            !_library.TryGetSound(ButtonType.Default, out clip, out volume))
            return;

        if (clip != null)
            customSource.PlayOneShot(clip, volume);
    }

    public void Play(ButtonType type)
    {
        Play(type, _audioSource);
    }

    public void Play(AudioClip clip, AudioSource customSource, float volume = 1f, bool loop = false, float fadeDuration = 1f)
    {
        if (!_isInitialize || clip == null || customSource == null) return;

        if (loop)
        {
            if (customSource.clip == clip && customSource.isPlaying) return;

            customSource.loop = true;
            _volumeFader.CrossFadeClip(customSource, clip, volume, fadeDuration);
        }
        else
        {
            customSource.PlayOneShot(clip, volume);
        }
    }
}


