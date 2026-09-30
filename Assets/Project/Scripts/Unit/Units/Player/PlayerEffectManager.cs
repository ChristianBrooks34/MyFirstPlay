using System;
using UnityEngine;


public class PlayerEffectManager : EffectManager, IDisposable
{
    private readonly Player _player;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly PlayerEventManager _playerEventManager;
    private readonly BloodSplashDeadEffect _bloodSplashDeadEffect;
    private readonly DamageBloodSplashEffectPool _damageBloodSplashEffectPool;

    private DamageFlash damageFlash;

    public PlayerEffectManager(Player player, SpriteRenderer spriteRenderer, PlayerEventManager playerEventManager,
        BloodSplashDeadEffect bloodSplashDeadEffect, DamageBloodSplashEffectPool damageBloodSplashEffectPool)
    {
        _player = player;
        _spriteRenderer = spriteRenderer;
        _playerEventManager = playerEventManager;
        _bloodSplashDeadEffect = bloodSplashDeadEffect;
        _damageBloodSplashEffectPool = damageBloodSplashEffectPool;

        _playerEventManager.OnDead += TriggerDeadEffect;
        _playerEventManager.OnDamage += TriggerDamageEffect;
        _playerEventManager.OnStartedAttack += TriggerAttackEffect;
    }


    public void TriggerAttackEffect()
    {

    }

    public void TriggerDamageEffect(float value)
    {
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

            go.transform.position = _player.PointDamageBloodSplash.position;

            go.transform.localRotation = Quaternion.Euler(0, 90, 0);

            go.transform.localScale = new Vector3(
                Mathf.Abs(go.transform.localScale.x),
                Mathf.Abs(go.transform.localScale.y),
                Mathf.Abs(go.transform.localScale.z));

            var bloodSplashDamageEffect = go.GetComponent<BloodSplashDamageEffect>();

            AddEffect(bloodSplashDamageEffect);

            Countdown(
                () =>
                {
                    RemoveEffect(bloodSplashDamageEffect);
                    _damageBloodSplashEffectPool.ReturnObject(go);
                },
                0.1f)
                .Forget();
        }
    }

    public void TriggerDeadEffect()
    {
        var go = GameObject.Instantiate(
            _bloodSplashDeadEffect, _player.PointDamageBloodSplash.position, Quaternion.identity, _player.transform);

        go.transform.localRotation = Quaternion.Euler(-90, 0, 0);

        IEffect bloodSplashDeadEffect = go.GetComponent<BloodSplashDeadEffect>();
        AddEffect(bloodSplashDeadEffect);

        Countdown(
            () => RemoveEffect(bloodSplashDeadEffect), 0.2f)
            .Forget();
    }

    public void Dispose()
    {
        _playerEventManager.OnDamage -= TriggerDamageEffect;
        _playerEventManager.OnStartedAttack -= TriggerAttackEffect;
        _playerEventManager.OnDead -= TriggerDeadEffect;
    }
}
