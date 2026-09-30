using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponProfile WeaponProfile;

    private Animator _animator;
    public Animator Animator
    {
        get
        {
            _animator = GetComponent<Animator>();

            return _animator;
        }
        private set
        {
            _animator = value;
        }
    }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (Animator == null)
        {
            Debug.LogError("Animator == null");
        }
    }
}
