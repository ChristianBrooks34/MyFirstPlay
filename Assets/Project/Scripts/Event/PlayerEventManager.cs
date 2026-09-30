using System;
using UnityEngine;

public class PlayerEventManager : UnitEventManager
{
    public event Action OnStartedAttack;
    public event Action OnCanseledAttack;

    public event Action<float> OnChangeHoldTimeAttack;

    public event Action OnRangeAttackStart;

    public event Action OnBuildingAttackStart;
    public event Action OnCircleBallAttackStart;

    public event Action<Collider2D> OnTriggerEnter;
    public event Action<Collider2D> OnTriggerExit;
    public event Action<Collider2D> OnTriggerStay;

    public event Action<IPickable> OnPickableEnter;

    public event Action OnMove;

    public event Action<Player> OnPlayerInitialize;

    public void TriggerMove() => OnMove?.Invoke();

    public void TriggerAttackStart() => OnStartedAttack?.Invoke();

    public void TriggerAttackCancel() => OnCanseledAttack?.Invoke();

    public void TriggerAttackChangeHoldTime(float value) => OnChangeHoldTimeAttack?.Invoke(value);

    public void TriggerRangeAttackStart() => OnRangeAttackStart?.Invoke();

    public void TriggerBuildingAttackStart() => OnBuildingAttackStart?.Invoke();
    public void TriggerCircleBallAttackStart() => OnCircleBallAttackStart?.Invoke();
    public void TriggerTriggerEnter2D(Collider2D collider)
    {
        OnTriggerEnter?.Invoke(collider);

        if (collider.TryGetComponent<IPickable>(out var pickable))
        {
            OnPickableEnter?.Invoke(pickable);
        }
    }

    public void TriggerTriggerExit2D(Collider2D collider) => OnTriggerExit?.Invoke(collider);

    public void TriggerTriggerStay2D(Collider2D collider) => OnTriggerStay?.Invoke(collider);

    public void TriggerPlayerInitialize(Player player) => OnPlayerInitialize?.Invoke(player);

}

