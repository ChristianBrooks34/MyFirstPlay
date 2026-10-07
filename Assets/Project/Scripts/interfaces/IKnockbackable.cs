using UnityEngine;

public interface IKnockbackable
{
    bool IsPushing { get; set; }
    void Knockback(Vector2 direction);
}

