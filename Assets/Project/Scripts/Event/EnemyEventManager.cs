using System;
using UnityEngine;

public class EnemyEventManager : UnitEventManager
{
    public event Action<GameObject> OnStartAttack;
    public event Action OnCanseledAttack;

    public event Action<Collider2D> OnTriggerEnter;
    public event Action<Collider2D> OnTriggerExit;
    public event Action<Collider2D> OnTriggerStay;

    public event Action OnMove;

    public void TriggerMove() => OnMove?.Invoke();

    public void TriggerStartedAttack(GameObject go) => OnStartAttack?.Invoke(go);

    public void TriggerCanceledAttack() => OnCanseledAttack?.Invoke();

    public void TriggerTriggerExit2D(Collider2D collider) => OnTriggerExit?.Invoke(collider);

    public void TriggerTriggerStay2D(Collider2D collider) => OnTriggerStay?.Invoke(collider);
}

