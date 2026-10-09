using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class HealthDrop : Drop, IPickable
{
    private Player _player;
    private Game _game;
    private VisualEffectManager _visualEffectManager;

    private VisualEffectTween _waveEffect;
    private VisualEffectTween _pulseEffect;
    private CancellationTokenSource _cts;

    [Inject]
    public void Construct(Game game, Player player, VisualEffectManager visualEffectManager,
        [Inject(Id = nameof(WaveVerticalEffect))] VisualEffectTween waveEffect,
        [Inject(Id = nameof(PulsateScaleEffect))] VisualEffectTween pulseEffect)
    {
        _game = game;
        _player = player;
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

        _player.Health.Add(DropProfile.BaseData.Count);
        Delete();
    }

    private void StartAnimation()
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        _visualEffectManager.PlayEffectAsync(_waveEffect, gameObject, _cts.Token).Forget();
        _visualEffectManager.PlayEffectAsync(_pulseEffect, gameObject, _cts.Token).Forget();
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
