using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VisualEffectManager
{
    private readonly AnimationCurvesConfig _curvesConfig;

    // Конфиг автоматически придет из Zenject
    public VisualEffectManager(AnimationCurvesConfig curvesConfig)
    {
        _curvesConfig = curvesConfig;
    }

    public async UniTask PlayEffectAsync(VisualEffectTween effect, GameObject target, CancellationToken token)
    {
        if (effect == null || target == null) return;

        try
        {
            await effect.PlayAsync(target, _curvesConfig, token);
        }
        catch (OperationCanceledException)
        {
            // Ожидаемый выход, когда объект уничтожается или подбирается
        }
    }
}
