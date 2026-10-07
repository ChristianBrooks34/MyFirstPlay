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
        Debug.LogError("KnockbackCoroutine 1");
        if (gameObject.IsPushing || gameObject == null) return;
        Debug.LogError("KnockbackCoroutine 2");
        if (!gameObject.CanBeAttacked)
        {
            return;
        }
        Debug.LogError("KnockbackCoroutine 3");

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
