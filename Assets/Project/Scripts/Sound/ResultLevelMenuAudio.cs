using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResultLevelMenuAudio : IDisposable
{
    private readonly ResultLevelMenu _resultLevelMenu;
    private readonly SoundManager _soundManager;
    private readonly ResultLevelMenuAudioProfile _resultLevelMenuAudioProfile;

    private AudioSource _audioSource;

    public ResultLevelMenuAudio(ResultLevelMenu resultLevelMenu, SoundManager soundManager, ResultLevelMenuAudioProfile resultLevelMenuAudioProfile)
    {
        _resultLevelMenu = resultLevelMenu;
        _soundManager = soundManager;
        _resultLevelMenuAudioProfile = resultLevelMenuAudioProfile;

        _audioSource = new GameObject("ResultLevelMenuAudioSource").AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        _resultLevelMenu.OnOpen += PlaySound;
    }

    private void PlaySound(bool isLevelWin)
    {
        if (isLevelWin)
        {
            _soundManager.Play(_resultLevelMenuAudioProfile.WinSound.Clip, _audioSource, _resultLevelMenuAudioProfile.WinSound.Value);
        }
        else
        {
            _soundManager.Play(_resultLevelMenuAudioProfile.LoseSound.Clip, _audioSource, _resultLevelMenuAudioProfile.LoseSound.Value);
        }
    }

    public void Dispose()
    {
        _resultLevelMenu.OnOpen -= PlaySound;
    }
}

