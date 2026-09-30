using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class VisualEffectManager
{
    private readonly AnimationCurvesConfig _animationCurvesConfig;

    public VisualEffectManager(AnimationCurvesConfig animationCurvesConfig)
    {
        _animationCurvesConfig = animationCurvesConfig;
    }

    public async UniTaskVoid PulsateScale(CancellationToken token, Transform transform)
    {
        try
        {
            var currentTime = 0f;

            var startScale = transform.localScale;

            while (true)
            {
                if (!transform.gameObject.activeInHierarchy) break;

                currentTime += Time.deltaTime;

                var num = _animationCurvesConfig.PulseScale.Evaluate(currentTime);

                Vector3 newScale =
                    new Vector3(
                        startScale.x + num,
                        startScale.y + num,
                        startScale.z + num);

                transform.localScale = newScale;

                await UniTask.DelayFrame(1);
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    public async UniTask WaveVectical(CancellationToken token, Transform transform)
    {
        try
        {
            var currentTime = 0f;
            var startPosition = transform.position;

            while (true)
            {
                if (!transform.gameObject.activeInHierarchy) break;

                currentTime += Time.deltaTime;

                Vector3 newPosition =
                    new Vector3(
                        startPosition.x,
                        startPosition.y + _animationCurvesConfig.WaveVertical.Evaluate(currentTime),
                        startPosition.z);

                transform.position = newPosition;

                await UniTask.DelayFrame(1);
            }
        }
        catch (OperationCanceledException)
        {

        }
    }
}
