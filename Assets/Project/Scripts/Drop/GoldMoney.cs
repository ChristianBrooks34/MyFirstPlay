using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GoldMoney : Drop, IPickable
{
    private Wallet _wallet;
    private VisualEffectManager _visualEffectManager;

    private VisualEffectTween _waveEffect;
    private VisualEffectTween _pulseEffect;
    private CancellationTokenSource _cts;

    [Inject]
    public void Construct(Wallet wallet, VisualEffectManager visualEffectManager,
        [Inject(Id = nameof(WaveVerticalEffect))] VisualEffectTween waveEffect,
        [Inject(Id = nameof(PulsateScaleEffect))] VisualEffectTween pulseEffect)
    {
        _wallet = wallet;
        _visualEffectManager = visualEffectManager;
        _waveEffect = waveEffect;
        _pulseEffect = pulseEffect;

        StartAnimation();
    }

    public bool CanBePicked()
    {
        return !IsCollected;
    }

    public void OnPicked()
    {
        if (!CanBePicked()) return;

        _wallet.Add(DropProfile.BaseData.Count);
        Delete();
    }

    private void StartAnimation()
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        InitAndPlayEffectsAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid InitAndPlayEffectsAsync(CancellationToken token)
    {
        await UniTask.DelayFrame(1, cancellationToken: token);

        _visualEffectManager.PlayEffectAsync(_waveEffect, gameObject, token).Forget();
        _visualEffectManager.PlayEffectAsync(_pulseEffect, gameObject, token).Forget();
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    private void Delete()
    {
        if (IsCollected) return;
        IsCollected = true;

        CleanUp();
        Destroy(gameObject);
    }

    private void CleanUp()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
