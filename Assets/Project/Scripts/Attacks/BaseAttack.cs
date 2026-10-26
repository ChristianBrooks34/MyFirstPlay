using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


public abstract class BaseAttack : MonoBehaviour
{
    public AttackData AttackData;

    public event Action OnAttack;

    protected bool canAttack = true;

    public virtual void Attack() { }

    public virtual void OnHit(GameObject gameObject) { }

    public virtual bool CanAttack() { return canAttack; }

    public async UniTaskVoid AttackLoop(float seconds, CancellationToken cancellationToken, Unit unit = null)
    {
        if (!CanAttack()) return;
        Debug.LogError("AttackLoop");

        canAttack = false;

        bool isCancelled = await UniTask.Delay((int)(seconds * 1000), cancellationToken: cancellationToken).SuppressCancellationThrow();

        if (!isCancelled)
        {
            canAttack = true;
            if (unit != null)
            {
                Debug.LogError("Cancel AttackLoop");

                unit.CurrentAttack = null;
            }
        }
    }

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }
}