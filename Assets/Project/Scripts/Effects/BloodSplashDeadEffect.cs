using UnityEngine;

public class BloodSplashDeadEffect : MonoBehaviour, IEffect
{
    public ParticleSystem BloodSplash { get; private set; }

    public bool IsActive
    {
        get;
        private set;
    }

    public void Awake()
    {
        BloodSplash = GetComponent<ParticleSystem>();
    }

    public void Apply()
    {
        BloodSplash.Play();

        IsActive = true;
    }

    public void Remove()
    {
        BloodSplash.Stop();

        IsActive = false;
    }
}
