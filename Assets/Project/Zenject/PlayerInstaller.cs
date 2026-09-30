using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private WeaponUpDownAnimationController _weaponAnimationController;
    [SerializeField] private BaseMove _baseMove;
    [SerializeField] private List<BaseAttack> _baseAttacks;

    private PlayerInput _playerInput;
    private bool _isInitialized = false;
    private PlayerEventManager _playerEventManager;

    public override void InstallBindings()
    {
        BindPlayerEventManager();

        Container
            .Bind<Transform>()
            .FromInstance(_player?.transform)
            .AsSingle();
    }

    public void BindAll()
    {
        if (_isInitialized) return;

        Debug.Log("PlayerInstaller: Starting full binding after initialization...");

        try
        {
            //if (_player == null)
            //{
            //    Debug.LogError("PlayerInstaller: _player is null!");
            //    return;
            //}

            //if (_player.UnitProfile == null || _player.UnitProfile.Data == null)
            //{
            //    Debug.LogError("PlayerInstaller: UnitProfile or Data is null in _player!");
            //    return;
            //}

            _playerInput = new PlayerInput();
            _playerInput.Enable();

            Container
                .Bind<Player>()
                .FromInstance(_player)
                .AsSingle()
                .NonLazy();

            BindHealth();
            BindAttack();
            BindOtherComponents();

            _isInitialized = true;
            Debug.Log("PlayerInstaller: Full binding completed successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayerInstaller: Error during full binding: {e.Message}");
        }
    }

    private void BindHealth()
    {
        try
        {
            //Container
            //    .Bind<Health>()
            //    .FromInstance(new Health(_player.UnitProfile.Data))
            //    .AsSingle();
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayerInstaller: Failed to bind Health: {e.Message}");
        }
    }

    private void BindAttack()
    {
        Container
            .Bind<List<BaseAttack>>()
            .FromInstance(_baseAttacks)
            .AsSingle();

        foreach (var attack in _baseAttacks)
        {
            if (attack != null)
            {
                Container
                    .Bind<BaseAttack>()
                    .FromInstance(attack)
                    .AsCached();
            }
        }
    }

    private void BindOtherComponents()
    {
        Container
            .Bind<UnitProfile>()
            .FromInstance(_player.UnitProfile)
            .AsSingle();

        Container
            .Bind<PlayerInput>()
            .FromInstance(_playerInput)
            .AsSingle();

        Container
            .BindInterfacesTo<PlayerController>()
            .AsCached()
            .NonLazy();

        Container
            .Bind<PlayerEffectManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<PlayerStatusEffectManager>()
            .AsSingle()
            .NonLazy();

        Container
            .Bind<BaseMove>()
            .FromInstance(_baseMove)
            .AsSingle();

        Container
            .Bind<DamageFlash>()
            .AsSingle();

        Container
            .Bind<SpriteRenderer>()
            .FromInstance(_spriteRenderer)
            .AsSingle();

        Container
            .BindInterfacesTo<PlayerAnimationController>()
            .AsCached();

        Container
            .Bind<Animator>()
            .FromInstance(_playerAnimator)
            .AsSingle();

        Container
            .BindInterfacesTo<DepthController>()
            .AsSingle()
            .WithArguments(_player)
            .NonLazy();

        Container
            .Bind<PlayerAttackController>()
            .AsSingle();

        Container
            .Bind<WeaponUpDownAnimationController>()
            .FromInstance(_weaponAnimationController)
            .AsSingle()
            .WithArguments(_player)
            .NonLazy();

        Container
            .Bind<Weapon>()
            .FromInstance(_player.Weapon)
            .AsSingle()
            .NonLazy();
    }

    private void BindPlayerEventManager()
    {
        _playerEventManager = new PlayerEventManager();
        //_playerEventManager.OnPlayerInitialize += (player) =>
        //{
        Debug.Log("PlayerInstaller: Received OnPlayerInitialize event");
        BindAll();
        //};

        Container
            .Bind<PlayerEventManager>()
            .FromInstance(_playerEventManager)
            .AsSingle();
    }
}

