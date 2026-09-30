using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class EffectManager
{
    private List<IEffect> _activeEffects = new List<IEffect>();

    public void AddEffect(IEffect effect)
    {
        if (!effect.IsActive)
        {
            effect.Apply();
            _activeEffects.Add(effect);
        }
    }

    public void RemoveEffect(IEffect effect)
    {
        if (effect.IsActive)
        {
            effect.Remove();
            _activeEffects.Remove(effect);
        }
    }

    public void RemoveAllEffect()
    {
        foreach (var effect in _activeEffects)
            effect.Remove();

        _activeEffects.Clear();
    }

    protected async UniTaskVoid Countdown(Action action, float seconds)
    {
        await UniTask.Delay((int)(1000 * seconds));
        action.Invoke();
    }
}
