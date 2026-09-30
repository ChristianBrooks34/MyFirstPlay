using UnityEngine;

public class BloodSplashDamageEffect : MonoBehaviour, IEffect
{
    private ParticleSystem _bloodSplash;

    public bool IsActive
    {
        get;
        private set;
    }

    public void Awake()
    {
        _bloodSplash = GetComponent<ParticleSystem>();
    }

    public void Apply()
    {
        _bloodSplash.Play();

        IsActive = true;
    }

    public void Remove()
    {
        _bloodSplash.Stop();

        IsActive = false;
    }
}
