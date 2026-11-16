using System;
using UnityEngine;
using Zenject;

public class SoundManager : IInitializable, IDisposable
{
    private SoundsEventManager _eventManager;
    private SoundLibrary _library;
    private AudioSource audioSource;

    public SoundManager(SoundsEventManager eventManager, SoundLibrary library)
    {
        _eventManager = eventManager;
        _library = library;
    }

    public void Initialize() => _eventManager.OnButtonClicked += Play;
    public void Dispose() => _eventManager.OnButtonClicked -= Play;

    private void Play(ButtonType type)
    {
        if (!_library.TryGetSound(type, out var clip, out var volume))
            _library.TryGetSound(ButtonType.Default, out clip, out volume);

        if (clip != null) audioSource.PlayOneShot(clip, volume);
    }
}


