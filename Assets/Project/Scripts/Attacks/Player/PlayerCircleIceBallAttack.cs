using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class PlayerCircleIceBallAttack : BallAttack
{
    [SerializeField] private int _countBall;

    [Inject] private DiContainer _diContainer;

    private PlayerEventManager _playerEventManager;
    private ProjectileContainer _projectileContainer;

    private bool _isSubscribed;

    [Inject]
    public void Construct(ProjectileContainer projectileContainer, PlayerEventManager playerEventManager)
    {
        _projectileContainer = projectileContainer;
        _playerEventManager = playerEventManager;

        ToggleSubscription(true);
    }

    private void OnEnable() => ToggleSubscription(true);
    private void OnDisable() => ToggleSubscription(false);
    private void OnDestroy() => ToggleSubscription(false);

    private void ToggleSubscription(bool subscribe)
    {
        if (_playerEventManager == null) return;

        if (subscribe)
        {
            if (_isSubscribed) return;
            _playerEventManager.OnCircleBallAttackStart += Attack;
            _isSubscribed = true;
        }
        else
        {
            if (!_isSubscribed) return;
            _playerEventManager.OnCircleBallAttackStart -= Attack;
            _isSubscribed = false;
        }
    }

    public override void Attack()
    {
        if (unit.CurrentAttack != null || !CanAttack()) return;

        unit.CurrentAttack = this;

        TriggerAttack();
        SpawnProjectile();
    }

    public override void SpawnProjectile(DiContainer diContainer = null, ProjectileContainer projectileContainer = null)
    {
        List<Vector3> positions = PositionGenerator.GetPointInCircle(_countBall, 0.1f, pointSpawnBall.position);

        foreach (var position in positions)
        {
            var ball = _diContainer.InstantiatePrefab(ballPrefab, position, Quaternion.identity, _projectileContainer.transform);

            ball.name = "Projectile " + Time.frameCount;

            var currentProjectile = ball.GetComponent<Projectile>();

            if (currentProjectile != null)
            {
                currentProjectile.Initialized(position - pointSpawnBall.position, unit);

                // ИЗМЕНЕНО: В анимацию передан токен отмены от уничтожения конкретного шара ball.GetCancellationTokenOnDestroy()
                ScaleUpBall(0f, currentProjectile.transform.localScale.x, 0.2f, currentProjectile, ball.GetCancellationTokenOnDestroy()).Forget();

                currentProjectile.OnExplosion += ResetUnitAttack;
            }

            // ИЗМЕНЕНО: Триггер-чекер теперь тоже локальный
            var currentTrigger = ball.GetComponent<ProjectileTrigerChecker>();
            if (currentTrigger != null)
            {
                currentTrigger.OnProjectileHit += HandleProjectileHit;
            }
        }
    }

    private void ResetUnitAttack()
    {
        if (unit != null) unit.CurrentAttack = null;
    }

    private async UniTaskVoid ScaleUpBall(float startScale, float endScale, float maxTimeInSeconds, Projectile ball, System.Threading.CancellationToken cancellationToken)
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

