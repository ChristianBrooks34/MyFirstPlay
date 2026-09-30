using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerIceBallAttack : BallAttack
{
    [Inject] private DiContainer _diContainer;

    private WeaponUpDownAnimationController _weaponUpDownAnimationController;
    private ProjectileContainer _projectileContainer;
    private Projectile _projectile;
    private ProjectileTrigerChecker _projectileTrigerChecker;

    [Inject]
    public void Construct(ProjectileContainer projectileContainer, WeaponUpDownAnimationController weaponUpDownAnimationController)
    {
        _projectileContainer = projectileContainer;
        _weaponUpDownAnimationController = weaponUpDownAnimationController;

        _weaponUpDownAnimationController.Attack += Attack;
    }

    private Vector3 GetMouseWordPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void SpawnProjectile(DiContainer diContainer = null, ProjectileContainer projectileContainer = null)
    {
        var ball = _diContainer.InstantiatePrefab(ballPrefab, pointSpawnBall.position, Quaternion.identity, _projectileContainer.transform);

        ball.name = "Projectile " + Time.frameCount;

        _projectile = ball.GetComponent<Projectile>();

        _projectile.Initialized(
            GetMouseWordPosition() - pointSpawnBall.position,
            unit);

        ScaleUpBall(0, _projectile.transform.localScale.x, 0.2f, _projectile).Forget();

        _projectileTrigerChecker = ball.GetComponent<ProjectileTrigerChecker>();

        if (_projectileTrigerChecker != null)
        {
            _projectileTrigerChecker.OnProjectileHit += HandleProjectileHit;
        }

        _projectile.OnExplosion += () => unit.CurrentAttack = null;

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
