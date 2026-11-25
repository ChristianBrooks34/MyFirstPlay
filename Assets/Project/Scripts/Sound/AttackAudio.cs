using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackAudio : IDisposable
{
    private protected List<BaseAttack> _baseAttacks;
    private protected SoundManager _soundManager;

    private AudioSource _audioSource;

    public AttackAudio(List<BaseAttack> baseAttacks, SoundManager soundManager)
    {
        _baseAttacks = baseAttacks;
        _soundManager = soundManager;

        _audioSource = new GameObject("AttackSound").AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        foreach (var baseAttack in _baseAttacks)
        {
            baseAttack.OnAttackStarted += PlayAttackStartSound;
            baseAttack.OnAttack += PlayAttackSound;
        }
    }

    private void PlayAttackStartSound(BaseAttack baseAttack)
    { 
        var baseSoundType = baseAttack.AttackData.AttackSoundProfile.GetRandomChargeClip();
        if (baseSoundType == null) return;
        _soundManager.Play(baseSoundType.Clip, _audioSource, baseSoundType.Value);
    }

    private void PlayAttackSound(BaseAttack baseAttack)
    {
        var baseSoundType = baseAttack.AttackData.AttackSoundProfile.GetRandomReleaseClip();
        if (baseSoundType == null) return;
        _soundManager.Play(baseSoundType.Clip, _audioSource, baseSoundType.Value);
    }

    public void Dispose()
    {
        foreach (var baseAttack in _baseAttacks)
        {
            baseAttack.OnAttackStarted -= PlayAttackStartSound;
            baseAttack.OnAttack -= PlayAttackSound;
        }
    }
}
