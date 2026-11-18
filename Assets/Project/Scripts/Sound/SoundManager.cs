using System;
using UnityEngine;
using Zenject;

public class SoundManager : IInitializable, IDisposable
{
    private SoundsEventManager _eventManager;
    private SoundLibrary _library;
    private AudioSource audioSource;
    private bool _isInitialize;

    public SoundManager(SoundsEventManager eventManager, SoundLibrary library)
    {
        Debug.LogError("SoundManager");
        _eventManager = eventManager;
        _library = library;
    }

    public void Initialize()
    {
        Debug.LogError("Initialize");

        audioSource = new GameObject("SoundManagerAudioSource").AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        GameObject.DontDestroyOnLoad(audioSource);

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
        Debug.LogError("Play");

        if (!_library.TryGetSound(type, out var clip, out var volume) &&
        !_library.TryGetSound(ButtonType.Default, out clip, out volume))
            return;

        if (clip != null) audioSource.PlayOneShot(clip, volume);
    }
}


