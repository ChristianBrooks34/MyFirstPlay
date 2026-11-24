using System;
using UnityEngine;

public class UnitAudio : IDisposable
{
    private readonly UnitAudioConfig _unitAudioConfig;
    private readonly AnimationEventReceiver _animationEventReceiver;
    private readonly UnitEventManager _unitEffectManager;
    private readonly SoundManager _soundManager;

    private AudioSource _audioSource;

    public UnitAudio(UnitAudioConfig unitAudioConfig, AnimationEventReceiver animationEventReceiver, UnitEventManager unitEffectManager,
        SoundManager soundManager)
    {
        _unitAudioConfig = unitAudioConfig;
        _animationEventReceiver = animationEventReceiver;
        _unitEffectManager = unitEffectManager;
        _soundManager = soundManager;

        _audioSource = new GameObject("UnitAudioSource").AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        _unitEffectManager.OnDead += PlayDeathAudio;
        _unitEffectManager.OnDamage += PlayTakeDamageAudio;
        _animationEventReceiver.OnFootsteps += PlayFootstepsAudio;
    }

    private void PlayFootstepsAudio()
    {
        _soundManager.Play(_unitAudioConfig.GetRandomFootstep(), _audioSource, _unitAudioConfig.Volume, false);
    }

    private void PlayTakeDamageAudio(float damage)
    {
        _soundManager.Play(_unitAudioConfig.GetRandomTakeDamage(), _audioSource, _unitAudioConfig.Volume, false);
    }

    private void PlayDeathAudio()
    {
        _soundManager.Play(_unitAudioConfig.GetRandomDeath(), _audioSource, _unitAudioConfig.Volume, false);
    }

    public void Dispose()
    {
        _unitEffectManager.OnDead -= PlayDeathAudio;
        _unitEffectManager.OnDamage -= PlayTakeDamageAudio;
        _animationEventReceiver.OnFootsteps -= PlayFootstepsAudio;
    }
}
