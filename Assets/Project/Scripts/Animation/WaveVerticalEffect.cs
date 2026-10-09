using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Wave Vertical", fileName = "NewWaveVerticalEffect")]
public class WaveVerticalEffect : VisualEffectTween
{
    public override async UniTask PlayAsync(GameObject target, AnimationCurvesConfig config, CancellationToken token)
    {
        if (target == null || config == null) return;

        var transform = target.transform;
        var startPosition = transform.position;
        var currentTime = 0f;

        while (!token.IsCancellationRequested)
        {
            if (!target.activeInHierarchy) break;

            currentTime += Time.deltaTime;
            var offset = config.WaveVertical.Evaluate(currentTime) * 10;

            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + offset,
                startPosition.z
            );

            // ћ€гко ждем один кадр Update
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}
