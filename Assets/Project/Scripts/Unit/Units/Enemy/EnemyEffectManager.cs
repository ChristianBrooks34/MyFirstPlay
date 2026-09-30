using System;
using UnityEngine;

public class EnemyEffectManager : EffectManager, IDisposable
{
    private readonly Enemy _enemy;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly DeadBloodSplashEffectPool _deadBloodSplashEffectPool;
    private readonly UnitEventManager _enemyEventManager;
    private readonly DamageBloodSplashEffectPool _damageBloodSplashEffectPool;

    private DamageFlash damageFlash;

    public EnemyEffectManager(Enemy enemy, SpriteRenderer spriteRenderer, DeadBloodSplashEffectPool deadBloodSplashEffectPool,
        UnitEventManager enemyEventManager, DamageBloodSplashEffectPool damageBloodSplashEffectPool)
    {
        _enemy = enemy;
        _spriteRenderer = spriteRenderer;
        _enemyEventManager = enemyEventManager;
        _deadBloodSplashEffectPool = deadBloodSplashEffectPool;
        _damageBloodSplashEffectPool = damageBloodSplashEffectPool;

        _enemyEventManager.OnDamage += TriggerDamageEffect;
        _enemyEventManager.OnDead += TriggerDeadEffect;
    }


    public void TriggerAttackEffect()
    {
        Debug.Log("PlayAttackEffect");
    }

    public void TriggerDamageEffect(float value)
    {
        if (_enemy == null || _enemy.IsDead || _spriteRenderer == null) return;
        if (_enemy.PointDamageBloodSplash == null) return;

        if (damageFlash == null)
        {
            damageFlash = new DamageFlash(_spriteRenderer, Color.white, Color.red, 0.1f);
        }
        AddEffect(damageFlash);

        Countdown(
            () => RemoveEffect(damageFlash), 0.1f)
            .Forget();

        if (_damageBloodSplashEffectPool != null)
        {
            var go = _damageBloodSplashEffectPool.GetObject();

            if (go == null) return;

            go.transform.position = _enemy.PointDamageBloodSplash.position;

            if (_enemy.PointDamageBloodSplash == null)
            {
                Debug.LogError("PointDamageBloodSplash == null");
                return;
            }

            go.transform.localRotation = Quaternion.Euler(0, 90 * (_enemy.transform.localScale.x > 0 ? 1 : -1), 0);

            var bloodSplashDamageEffect = go.GetComponent<BloodSplashDamageEffect>();

            AddEffect(bloodSplashDamageEffect);

            Countdown(
                () =>
                {
                    RemoveEffect(bloodSplashDamageEffect);
                    _damageBloodSplashEffectPool.ReturnObject(go);
                }
                , 0.5f)
                .Forget();
        }
    }

    public void TriggerDeadEffect()
    {
        var go = _deadBloodSplashEffectPool.GetObject();

        if (go == null) return;

        go.transform.position = _enemy.PointDeadBloodSplash.position;

        if (_enemy.PointDeadBloodSplash == null)
        {
            Debug.LogError("PointDamageBloodSplash == null");
            return;
        }

        go.transform.localScale = Vector3.one * UnitSizeSettings.SizeScales[_enemy.UnitProfile.BaseData.SizeType] / 3;

        go.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        IEffect bloodSplashDeadEffect = go.GetComponent<BloodSplashDeadEffect>();

        AddEffect(bloodSplashDeadEffect);

        Countdown(
            () =>
            {
                RemoveEffect(bloodSplashDeadEffect);
                _deadBloodSplashEffectPool.ReturnObject(go);
            }
            , 0.5f)
            .Forget();
    }

    public void Dispose()
    {
        _enemyEventManager.OnDamage -= TriggerDamageEffect;
        _enemyEventManager.OnDead -= TriggerDeadEffect;
    }
}