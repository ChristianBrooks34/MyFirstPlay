using Cysharp.Threading.Tasks;
using UnityEngine;

public static class KnockbackControoller
{
    public static void Knockback(IKnockbackable knockbackable, Vector2 direction)
    {
        knockbackable.Knockback(direction);
    }

    public static async UniTaskVoid KnockbackCoroutine<T>(Vector2 direction, T gameObject, KnockbackData knockbackData) where T : Component, IDamageable, IKnockbackable
    {
        if (gameObject.IsPushing || gameObject == null) return;
        if (!gameObject.CanBeAttacked)
        {
            return;
        }

        gameObject.IsPushing = true;
        var currentTime = 0f;

        var targetOffset = direction * knockbackData.KnockbackDistance;

        Vector3 pushVelocity = targetOffset / knockbackData.KnockbackTime;

        while (currentTime < knockbackData.KnockbackTime)
        {
            if (gameObject == null) break;

            gameObject.transform.position += pushVelocity * Time.deltaTime;

            currentTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }

        gameObject.IsPushing = false;
    }
}
