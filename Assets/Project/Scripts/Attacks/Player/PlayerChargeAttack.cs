using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class PlayerChargeAttack : BaseAttack, IAnimation
{
    [SerializeField] private float _minDamage;
    [SerializeField] private float _maxDamage;
    [SerializeField] private float _endScale;

    private Player _player;
    private LayerMask _layerMask;
    private Vector3 _startLocalScale;
    private float _currentHoldTime = 0;
    private CancellationTokenSource _cts;
    private AnimationCurve _animationCurve;
    private SpriteRenderer _weaponSpriteRenderer;
    private PlayerEventManager _playerEventManager;
    private PlayerAttackController _playerAttackController;
    private WeaponUpDownAnimationController _swordAnimationController;

    [SerializeField] private float _maxHoldTime;
    public float MaxHoldTime
    {
        get => _maxHoldTime;
        private set => _maxHoldTime = value;
    }

    [SerializeField] private float _mixHoldTime;
    public float MixHoldTime
    {
        get => _mixHoldTime;
        private set => _mixHoldTime = value;
    }

    public event Action OnStartedCharge;
    public event Action OnCanceledCharge;
    public event Action<float> OnChangeCharge;

    [Inject]
    public void Construct(Player player, AnimationCurve animationCurve, PlayerAttackController playerAttackController,
        WeaponUpDownAnimationController swordAnimationController, PlayerEventManager playerEventManager)
    {
        _player = player;
        _animationCurve = animationCurve;
        _playerEventManager = playerEventManager;
        _layerMask = LayerMask.NameToLayer("Enemy"); // ??
        _playerAttackController = playerAttackController;
        _swordAnimationController = swordAnimationController;

        _weaponSpriteRenderer = _player.Weapon.GetComponentInChildren<SpriteRenderer>();

        _startLocalScale = _weaponSpriteRenderer.transform.localScale;

        _swordAnimationController.Attack += Attack;

        _swordAnimationController.OnAttackButtonHeld += StartAnimation;
        _playerEventManager.OnCanseledAttack += CancelAnimateDiagonalWaveScale;
        _swordAnimationController.OnCancelSwordDownAnimation += CancelAnimation;
    }

    public override void Attack()
    {
        if (!(_player.CurrentAttack is PlayerChargeAttack)) return;
        if (!CanAttack) return;

        TriggerAttack();

        var colliders = AttackHitChecker.GetFrontTargetsInAttackRange(
            _player.transform, _layerMask, AttackData.AttackRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                OnHit(collider);
            }
        }
    }

    public override void OnHit(GameObject gameObject)
    {
        var enemy = gameObject.GetComponent<Enemy>();

        CalculateDamage();

        enemy.ApplyDamage(_player.CalculateTotalDamage());

        ReturnStartDamage();

        KnockbackControoller.Knockback
        (
            enemy,
            enemy.transform.position - _player.transform.position
        );
    }

    public void StartAnimation()
    {
        if (!CanAttack)
        {
            _swordAnimationController.StartAnimationSwordDown();
            return;
        }
        if (_playerAttackController.ButtonHoldTime < MixHoldTime) return;
        if (_player.CurrentAttack == this) return;

        _player.CurrentAttack = this;

        OnStartedCharge?.Invoke();

        _cts = new CancellationTokenSource();
        AnimateDiagonalWaveScale(_cts.Token).Forget();
    }

    private void CancelAnimation()
    {
        if (_player.CurrentAttack != this) return;

        _weaponSpriteRenderer.transform.localScale = new Vector3(_startLocalScale.x, _startLocalScale.y, _startLocalScale.z);

        //AttackLoop(AttackData.Cooldown).Forget();
    }

    public void CancelAnimateDiagonalWaveScale()
    {
        if (_cts == null) return;

        OnCanceledCharge?.Invoke();
        _swordAnimationController.CancelSwordUpAnimation();

        _cts.Cancel();
    }

    private async UniTaskVoid AnimateDiagonalWaveScale(CancellationToken token)
    {
        try
        {
            _currentHoldTime = 0f;

            while (_currentHoldTime < MaxHoldTime)
            {
                token.ThrowIfCancellationRequested();

                var normalizeHoldTime = _currentHoldTime / MaxHoldTime;

                _weaponSpriteRenderer.transform.localScale = new Vector3(
                    Mathf.Lerp(_startLocalScale.x, _endScale, _animationCurve.Evaluate(normalizeHoldTime)),
                    Mathf.Lerp(_startLocalScale.x, _endScale, _animationCurve.Evaluate(normalizeHoldTime)),
                    0);

                _currentHoldTime += Time.deltaTime;

                OnChangeCharge?.Invoke(_currentHoldTime);

                await UniTask.DelayFrame(1);
            }

        }
        catch (OperationCanceledException)
        {

        }
    }

    private void CalculateDamage()
    {
        var normalizeHoldTime = _currentHoldTime / MaxHoldTime;
        AttackData.Damage = Mathf.Lerp(_minDamage, _maxDamage, normalizeHoldTime);
    }

    private void ReturnStartDamage()
    {
        AttackData.Damage = _minDamage;
    }
}
