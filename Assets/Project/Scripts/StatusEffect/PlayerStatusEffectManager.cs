using System;

public class PlayerStatusEffectManager : EffectManager, IDisposable
{
    private readonly Player _player;
    private readonly PlayerEventManager _playerEventManager;

    public PlayerStatusEffectManager(PlayerEventManager playerEventManager, Player player)
    {
        _player = player;
        _playerEventManager = playerEventManager;

        _playerEventManager.OnDamage += TriggerDamageStatusEffect;
    }

    public void TriggerDamageStatusEffect(float value)
    {
        IEffect invincible = new Invincible(_player);

        AddEffect(invincible);

        Countdown(
            () => RemoveEffect(invincible), 1f)
            .Forget();
    }

    public void Dispose()
    {
        _playerEventManager.OnDamage -= TriggerDamageStatusEffect;
    }
}
