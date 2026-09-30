using Cysharp.Threading.Tasks;
using System;
using UnityEngine;


public abstract class BaseAttack : MonoBehaviour
{
    public AttackData AttackData;
    public bool CanAttack { get; protected set; } = true;

    public event Action OnAttack;

    public virtual void Attack() { }

    public virtual void OnHit(GameObject gameObject) { }

    public async UniTaskVoid AttackLoop(float seconds)
    {
        if (!CanAttack) return;
        CanAttack = false;
        await UniTask.Delay((int)(seconds * 1000));
        if (CanAttack == false) CanAttack = true;
    }

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }
}