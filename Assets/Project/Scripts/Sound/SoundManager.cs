using System;
using UnityEngine;
using Zenject;

public class SoundManager : IInitializable, IDisposable
{
    private SoundsEventManager _eventManager;
    private SoundLibrary _library;
    private AudioSource _audioSource;
    private bool _isInitialize;

    public SoundManager(SoundsEventManager eventManager, SoundLibrary library)
    {
        _eventManager = eventManager;
        _library = library;
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

    private void Play(ButtonType type)
    {
        if (!_isInitialize)
        {
            Debug.LogError("_isInitialize = false");
            return;
        }

        if (!_library.TryGetSound(type, out var clip, out var volume) &&
        !_library.TryGetSound(ButtonType.Default, out clip, out volume))
            return;

        if (clip != null) _audioSource.PlayOneShot(clip, volume);
    }
}


