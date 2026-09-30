using UnityEngine;

public static class KnockbackControoller
{
    public static void Knockback(IKnockbackable knockbackable, Vector2 direction)
    {
        knockbackable.Knockback(direction);
    }
}
