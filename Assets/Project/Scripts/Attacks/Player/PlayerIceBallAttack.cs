using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class PlayerIceBallAttack : BallAttack
{
    [Inject] private DiContainer _diContainer;

    private WeaponUpDownAnimationController _weaponUpDownAnimationController;
    private ProjectileContainer _projectileContainer;
    private Camera _mainCamera;
    private bool _isSubscribed;

    [Inject]
    public void Construct(ProjectileContainer projectileContainer, WeaponUpDownAnimationController weaponUpDownAnimationController)
    {
        _projectileContainer = projectileContainer;
        _weaponUpDownAnimationController = weaponUpDownAnimationController;

        _weaponUpDownAnimationController.Attack += Attack;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable() => ToggleSubscription(true);
    private void OnDisable() => ToggleSubscription(false);
    private void OnDestroy() => ToggleSubscription(false);

    private void ToggleSubscription(bool subscribe)
    {
        if (_weaponUpDownAnimationController == null) return;

        if (subscribe)
        {
            if (_isSubscribed) return;
            _weaponUpDownAnimationController.Attack += Attack;
            _isSubscribed = true;
        }
        else
        {
            if (!_isSubscribed) return;
            _weaponUpDownAnimationController.Attack -= Attack;
            _isSubscribed = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void SpawnProjectile(DiContainer diContainer = null, ProjectileContainer projectileContainer = null)
    {
        if (pointSpawnBall == null || ballPrefab == null || _projectileContainer == null) return;

        var ball = _diContainer.InstantiatePrefab(ballPrefab, pointSpawnBall.position, Quaternion.identity, _projectileContainer.transform);

        ball.name = "Projectile " + Time.frameCount;

        var currentProjectile = ball.GetComponent<Projectile>();

        if (currentProjectile != null)
        {
            currentProjectile.Initialized(GetMouseWorldPosition() - pointSpawnBall.position, unit);

            ScaleUpBall(0f, currentProjectile.transform.localScale.x, 0.2f, currentProjectile, ball.GetCancellationTokenOnDestroy()).Forget();

            currentProjectile.OnExplosion += ResetUnitAttack;
        }

        var currentTrigger = ball.GetComponent<ProjectileTrigerChecker>();

        if (currentTrigger != null)
        {
            currentTrigger.OnProjectileHit += HandleProjectileHit;
        }
    }

    private void ResetUnitAttack()
    {
        if (unit != null) unit.CurrentAttack = null;
    }

    private async UniTaskVoid ScaleUpBall(float startScale, float endScale, float maxTimeInSeconds, Projectile ball,
        CancellationToken cancellationToken)
    {
        float currentTime = 0f;
        var coefficient = 1f / maxTimeInSeconds;

        while (currentTime < maxTimeInSeconds)
        {
            if (cancellationToken.IsCancellationRequested || ball == null || ball.gameObject == null) return;

            var scale = Mathf.Lerp(startScale, endScale, coefficient * currentTime);
            ball.transform.localScale = new Vector3(scale, scale, scale);

            currentTime += Time.deltaTime;

            bool isCancelled = await UniTask.DelayFrame(1, cancellationToken: cancellationToken).SuppressCancellationThrow();
            if (isCancelled) return;
        }
    }
}
