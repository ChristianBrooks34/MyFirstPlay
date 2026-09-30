using System.Collections.Generic;

public enum WeaponUpDownAnimation
{
    Up,
    Down,
    Idle
}

public class WeaponUpDownAnimator : AnimatorBase<WeaponUpDownAnimation>
{
    public override Dictionary<WeaponUpDownAnimation, string> TriggerNames { get; protected set; } = new Dictionary<WeaponUpDownAnimation, string>()
    {
        [WeaponUpDownAnimation.Up] = "Up",
        [WeaponUpDownAnimation.Down] = "Down",
        [WeaponUpDownAnimation.Idle] = "Idle"
    };
}

