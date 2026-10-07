using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class BuildingStatusEffectManager : EffectManager
{
    private readonly Building _building;
    private readonly BuildingEventManager _buildingEventManager;

    private IEffect _invincibleEffect;

    private CancellationTokenSource _cts;

    public BuildingStatusEffectManager(BuildingEventManager buildingEventManager, Building building)
    {
        _building = building;
        _buildingEventManager = buildingEventManager;

        _buildingEventManager.OnDamage += TriggerDamageStatusEffect;
    }

    public void TriggerDamageStatusEffect(float value)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (_invincibleEffect == null)
        {
            _invincibleEffect = new Invincible(_building);
            AddEffect(_invincibleEffect);
        }

        StartCountdownAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid StartCountdownAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token)
                         .AttachExternalCancellation(_building.GetCancellationTokenOnDestroy());

            if (_invincibleEffect != null)
            {
                RemoveEffect(_invincibleEffect);

                _invincibleEffect = null;
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    public void Dispose()
    {
        _buildingEventManager.OnDamage -= TriggerDamageStatusEffect;

        _cts?.Cancel();
        _cts?.Dispose();
    }
}
