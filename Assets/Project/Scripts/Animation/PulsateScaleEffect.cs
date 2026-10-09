using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Pulsate Scale", fileName = "NewPulsateScaleEffect")]
public class PulsateScaleEffect : VisualEffectTween
{
    public override async UniTask PlayAsync(GameObject target, AnimationCurvesConfig config, CancellationToken token)
    {
        if (target == null || config == null) return;

        var transform = target.transform;
        var startScale = transform.localScale;
        var currentTime = 0f;

        while (!token.IsCancellationRequested)
        {
            if (!target.activeInHierarchy) break;

            currentTime += Time.deltaTime;
            var offset = config.PulseScale.Evaluate(currentTime);

            transform.localScale = new Vector3(
                startScale.x + offset,
                startScale.y + offset,
                startScale.z + offset
            );

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}
