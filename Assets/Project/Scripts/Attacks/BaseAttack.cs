using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


public abstract class BaseAttack : MonoBehaviour
{
    public AttackData AttackData;
    public bool CanAttack { get; protected set; } = true;

    public event Action OnAttack;

    public virtual void Attack() { }

    public virtual void OnHit(GameObject gameObject) { }

    public async UniTaskVoid AttackLoop(float seconds, CancellationToken cancellationToken)
    {
        if (!CanAttack) return;

        CanAttack = false;

        bool isCancelled = await UniTask.Delay((int)(seconds * 1000), cancellationToken: cancellationToken).SuppressCancellationThrow();

        if (!isCancelled)
        {
            CanAttack = true;
        }
    }

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }
}