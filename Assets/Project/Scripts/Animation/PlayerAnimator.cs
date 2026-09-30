using System.Collections.Generic;

public enum PlayerAnimation
{
    Run,
    Dead,
    Idle
}

public class PlayerAnimator : AnimatorBase<PlayerAnimation>
{
    public override Dictionary<PlayerAnimation, string> TriggerNames { get; protected set; } = new Dictionary<PlayerAnimation, string>()
    {
        [PlayerAnimation.Run] = "Run",
        [PlayerAnimation.Dead] = "Dead",
        [PlayerAnimation.Idle] = "Idle"
    };
}
