using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController
{
    public bool IsButtonHeld { get; private set; }

    private PlayerInput _playerInput;
    private float _maxHoldTime = 3f;
    private CancellationTokenSource _cts;
    private PlayerEventManager _playerEventManager;
    public List<BaseAttack> BaseAttacks { get; private set; } = new List<BaseAttack>();

    public float ButtonHoldTime { get; private set; }

    public PlayerAttackController(PlayerInput playerInput, List<BaseAttack> baseAttacks,
        PlayerEventManager playerEventManager)
    {
        BaseAttacks = baseAttacks;
        _playerInput = playerInput;
        _playerEventManager = playerEventManager;

        _playerInput.Player.Attack.started += OnAttackStarted;
        _playerInput.Player.Attack.canceled += OnAttackCanceled;

        _playerInput.Player.RangeAttack.started += OnRangeAttackStarted;
        _playerInput.Player.SpawnBuildingAttack.started += OnBuildingAttack;
        _playerInput.Player.CircleBallAttack.started += OnCircleBallAttack;
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        _playerEventManager.TriggerAttackStart();

        _cts = new CancellationTokenSource();
        ButtonHoldTimerRoutine(_cts.Token).Forget();
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        ButtonHoldTime = 0f;
        Cancel();
    }

    private void OnRangeAttackStarted(InputAction.CallbackContext context)
    {
        _playerEventManager.TriggerRangeAttackStart();
    }

    private void OnBuildingAttack(InputAction.CallbackContext callbackContext)
    {
        _playerEventManager.TriggerBuildingAttackStart();
    }

    private void OnCircleBallAttack(InputAction.CallbackContext callbackContext)
    {
        _playerEventManager.TriggerCircleBallAttackStart();
    }

    private async UniTaskVoid ButtonHoldTimerRoutine(CancellationToken token)
    {
        if (IsButtonHeld) return;
        IsButtonHeld = true;

        try
        {
            while (ButtonHoldTime < _maxHoldTime)
            {
                token.ThrowIfCancellationRequested();
                ButtonHoldTime += Time.deltaTime;

                _playerEventManager.TriggerAttackChangeHoldTime(ButtonHoldTime);

                await UniTask.DelayFrame(1);
            }
        }
        catch (OperationCanceledException)
        {

        }

        IsButtonHeld = false;
    }

    private void Cancel()
    {
        _cts.Cancel();

        _playerEventManager.TriggerAttackCancel();
    }
}
