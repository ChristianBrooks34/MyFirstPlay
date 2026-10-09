using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public abstract class VisualEffectTween : ScriptableObject
{
    public abstract UniTask PlayAsync(GameObject target, AnimationCurvesConfig config, CancellationToken token);
}
