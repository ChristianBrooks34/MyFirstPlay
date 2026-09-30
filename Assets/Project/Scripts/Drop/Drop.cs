using System;
using UnityEngine;

public abstract class Drop : MonoBehaviour // нужен рефакторинг
{
    [SerializeField] private AnimationCurve _waveVerticalAnimation;
    [SerializeField] private AnimationCurve _pulsateScaleAnimation;

    public bool _isDead { get; set; } // дроп не может быть мертвым
    [NonSerialized] public DropProfile DropProfile;
}
