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
    private Projectile _projectile;
    private ProjectileTrigerChecker _projectileTrigerChecker;

    [Inject]
    public void Construct(ProjectileContainer projectileContainer, PlayerEventManager playerEventManager)
    {
        _projectileContainer = projectileContainer;
        _playerEventManager = playerEventManager;

        _playerEventManager.OnCircleBallAttackStart += Attack;
    }

    public override void Attack()
    {
        if (unit.CurrentAttack != null || !CanAttack) return;

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

            _projectile = ball.GetComponent<Projectile>();

            _projectile.Initialized(
                position - pointSpawnBall.position,
                unit);

            ScaleUpBall(0, _projectile.transform.localScale.x, 0.2f, _projectile).Forget();

            _projectileTrigerChecker = ball.GetComponent<ProjectileTrigerChecker>();

            if (_projectileTrigerChecker != null)
            {
                _projectileTrigerChecker.OnProjectileHit += HandleProjectileHit;
            }

            _projectile.OnExplosion += () => unit.CurrentAttack = null;

        }
        //AttackLoop(AttackData.Cooldown).Forget();
    }

    private async UniTaskVoid ScaleUpBall(float startScale, float endScale, float maxTimeInSeconds, Projectile ball)
    {
        float currentTime = 0f;
        var coefficent = 1 / maxTimeInSeconds;

        while (currentTime < maxTimeInSeconds)
        {
            var scale = Mathf.Lerp(startScale, endScale, coefficent * currentTime);

            ball.transform.localScale = new Vector3(scale, scale, scale);

            currentTime += Time.deltaTime;

            await UniTask.DelayFrame(1);
        }
    }
}

