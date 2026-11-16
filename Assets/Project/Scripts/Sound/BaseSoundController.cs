using UnityEngine;

public class BaseSoundController
{

}

public class SoundUIController : BaseSoundController
{
    public SoundUIController()
    {
        
    }

    public void Play(AudioSource audioSource, AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}


