using UnityEngine;
using Zenject;


public class RedirectingProjectile : Projectile
{
    [SerializeField] private float _homingDuration = 1f;
    [SerializeField] private float _homingDelay = 0.1f;
    public override ProjectileContainer ProjectileContainer { get; protected set; }
    public override BaseProjectileMovement ProjectileMovement { get; protected set; }

    private float _homingTimer;
    private bool _isHomingActive = true;
    private bool _isHomingStarted = false;

    [Inject]
    public void Construct(ProjectileContainer projectileContainer)
    {
        ProjectileContainer = projectileContainer;

        ProjectileMovement = GetComponent<RedirectingProjectileMovement>();

        ProjectileContainer.Add(ProjectileMovement);

        ProjectileTrigerChecker = GetComponent<ProjectileTrigerChecker>();
        Explosion.OnCanceled += OnExplosionCanceled;


        Explosion.OnCanceled += () => Destroy(gameObject);
        OnExplosion += () => GetComponent<SpriteRenderer>().enabled = false;

        TimeLiveCooldown().Forget();
    }

    public override void Initialized(Vector2 direction, Unit sourceUnit = null)
    {
        if (sourceUnit != null) Profile.BaseData.SourceUnit = sourceUnit;
        Direction = direction;
        _homingTimer = 0f;
        _isHomingStarted = false;
        TriggerInitialize();
    }

    private void Update()
    {
        UpdateDirectionToTarget();
    }

    private void UpdateDirectionToTarget()
    {
        if (!_isHomingStarted)
        {
            _homingTimer += Time.deltaTime;
            if (_homingTimer >= _homingDelay)
            {
                _isHomingStarted = true;
                _homingTimer = 0f;
            }
            return;
        }

        _homingTimer += Time.deltaTime;

        if (_homingTimer >= _homingDuration)
        {
            _isHomingActive = false;
            return;
        }

        Vector2 toPlayer = (Target.transform.position - transform.position).normalized;

        Vector2 newDirection = Vector2.Lerp(
            Direction,
            toPlayer,
            _homingTimer / _homingDuration
        );

        Direction = newDirection;
    }
}

