using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorBase<T> where T : Enum
{
    public abstract Dictionary<T, string> TriggerNames { get; protected set; }

    public void SetBool(Animator animator, T animation, bool value)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            animator.SetBool(nameAnimation, value);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
        }
    }

    public void SetTrigger(Animator animator, T animation)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            animator.SetTrigger(nameAnimation);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
        }
    }

    public void SetFloat(Animator animator, T animation, float value)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            animator.SetFloat(nameAnimation, value);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
        }
    }

    public void SetInt(Animator animator, T animation, int value)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            animator.SetInteger(nameAnimation, value);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
        }
    }

    public bool? GetBool(Animator animator, T animation)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            return animator.GetBool(nameAnimation);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
            return null;
        }
    }

    public float? GetFloat(Animator animator, T animation)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            return animator.GetFloat(nameAnimation);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
            return null;
        }
    }

    public int? GetInt(Animator animator, T animation)
    {
        if (TriggerNames == null)
            throw new InvalidOperationException("TriggerNames не инициализирован");

        string nameAnimation;

        if (TriggerNames.TryGetValue(animation, out nameAnimation))
        {
            return animator.GetInteger(nameAnimation);
        }
        else
        {
            Debug.LogWarning($"Анимации {animation} не существует в аниматоре {animator}");
            return null;
        }
    }
}
