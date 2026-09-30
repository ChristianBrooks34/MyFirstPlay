using System;
using UnityEngine;
using Zenject;

public class WeaponUpDownAnimationController : MonoBehaviour
{
    public bool UseHoldTime;

    private Player _player;
    private Animator _animator;
    private WeaponUpDownAnimator _weaponAnimator;
    private PlayerEventManager _playerEventManager;
    private PlayerAttackController _playerAttackController;

    public event Action OnStartedSwordDownAnimation;
    public event Action OnCancelSwordDownAnimation;
    public event Action OnStartedSwordUpAnimation;
    public event Action OnCancelSwordUpAnimation;
    public event Action OnAttackButtonHeld;
    public event Action Attack;

    public bool CanAttack
    {
        get
        {
            return _animator
                .GetCurrentAnimatorStateInfo(0)
                .IsName(_weaponAnimator.TriggerNames[WeaponUpDownAnimation.Idle]);
        }
    }

    [Inject]
    public void Construct(Player player, PlayerAttackController playerAttackController,
        WeaponUpDownAnimator weaponAnimator, PlayerEventManager playerEventManager)
    {
        _player = player;
        _playerAttackController = playerAttackController;
        _weaponAnimator = weaponAnimator;
        _playerEventManager = playerEventManager;
        _animator = _player.Weapon.Animator;

        _playerEventManager.OnStartedAttack += StartAnimationSwordUp;
    }

    public void StartAnimationSwordUp()
    {
        if (_player.CurrentAttack is PlayerStandardAttack)
        {
            if (!_player.CurrentAttack.CanAttack)
            {
                return;
            }
        }

        if (_player.IsDead) return;

        if (_animator == null)
        {
            Debug.LogError("Animator reference is not set!");
            return;
        }

        if (_animator.runtimeAnimatorController == null)
        {
            Debug.LogError("Animator has no controller assigned!");
            return;
        }

        if (_animator.isInitialized == false)
        {
            Debug.LogError("Animator is initialized == false");
        }

        var clipInfo = _animator.GetCurrentAnimatorStateInfo(0);
        var flag = false;

        if (clipInfo.IsName(_weaponAnimator.TriggerNames[WeaponUpDownAnimation.Idle]))
        {
            flag = true;
        }

        if (flag)
        {
            _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Up, true);
        }
        else
        {
            if (_weaponAnimator.GetBool(_animator, WeaponUpDownAnimation.Up) == true)
            {
                _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Up, false);

                StartAnimationSwordDown();
            }
            return;
        }

        OnStartedSwordUpAnimation?.Invoke();
    }

    public void StartAnimationSwordDown()
    {
        var clipInfo = _animator.GetCurrentAnimatorStateInfo(0);
        var flag = false;

        if (clipInfo.IsName(_weaponAnimator.TriggerNames[WeaponUpDownAnimation.Up]))
        {
            flag = true;
        }

        if (flag)
        {
            _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Up, false);
            _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Down, true);
        }
        else
        {
            if (_weaponAnimator.GetBool(_animator, WeaponUpDownAnimation.Down) == true)
            {
                _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Down, false);
            }
        }
    }

    public void CancelSwordUpAnimation()
    {
        var clipInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (clipInfo.IsName(_weaponAnimator.TriggerNames[WeaponUpDownAnimation.Up]))
        {
            if (_playerAttackController.ButtonHoldTime != 0 && UseHoldTime)
            {
                OnAttackButtonHeld?.Invoke();
            }
            else
            {
                _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Up, false);

                StartAnimationSwordDown();
                OnCancelSwordUpAnimation?.Invoke();
            }
        }
    }

    public void CancelSwordDownAnimation()
    {
        _weaponAnimator.SetBool(_animator, WeaponUpDownAnimation.Down, false);
        OnCancelSwordDownAnimation?.Invoke();
    }

    public void OnStartSwordUpAnimation()
    {
        OnStartedSwordUpAnimation?.Invoke();
    }

    public void OnStartSwordDownAnimation()
    {
        OnStartedSwordDownAnimation?.Invoke();
    }

    public void OnAttack()
    {
        Attack?.Invoke();
    }
}
