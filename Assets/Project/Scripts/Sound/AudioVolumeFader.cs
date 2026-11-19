using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AudioVolumeFader : IDisposable
{
    private readonly Dictionary<AudioSource, IDisposable> _activeFades = new();

    public void FadeTo(AudioSource source, float targetVolume, float duration, Action onComplete = null)
    {
        if (source == null) return;

        CancelFade(source);

        if (duration <= 0f)
        {
            source.volume = targetVolume;
            onComplete?.Invoke();
            return;
        }

        float startVolume = source.volume;
        float elapsed = 0f;

        var subscription = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (source == null)
                {
                    CancelFade(source);
                    return;
                }

                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                source.volume = Mathf.Lerp(startVolume, targetVolume, t);

                if (t >= 1f)
                {
                    source.volume = targetVolume;
                    CancelFade(source);
                    onComplete?.Invoke();
                }
            });

        _activeFades[source] = subscription;
    }

    public void CrossFadeClip(AudioSource source, AudioClip newClip, float targetVolume, float duration)
    {
        if (source == null || newClip == null) return;

        float halfDuration = duration * 0.5f;

        if (source.isPlaying && source.volume > 0.01f)
        {
            FadeTo(source, 0f, halfDuration, () =>
            {
                source.Stop();
                source.clip = newClip;
                source.Play();
                FadeTo(source, targetVolume, halfDuration);
            });
        }
        else
        {
            source.Stop();
            source.clip = newClip;
            source.volume = 0f;
            source.Play();
            FadeTo(source, targetVolume, halfDuration);
        }
    }

    public void CancelFade(AudioSource source)
    {
        if (source != null && _activeFades.TryGetValue(source, out var sub))
        {
            sub.Dispose();
            _activeFades.Remove(source);
        }
    }

    public void Dispose()
    {
        foreach (var sub in _activeFades.Values)
            sub.Dispose();

        _activeFades.Clear();
    }
}

