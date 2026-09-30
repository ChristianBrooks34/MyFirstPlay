using UnityEngine;
using Zenject;

public class SpawnerAttackSlot
{
    private readonly GameObject _attackSlotProfab;
    private readonly Transform _parentForAttackSlots;
    private DiContainer _container;

    public SpawnerAttackSlot(SpawnUnitEventManager spawnUnitEventManager, GameObject attackSlotProfab,
        DiContainer container, Transform parentForAttackSlots)
    {
        _container = container;
        _attackSlotProfab = attackSlotProfab;
        _parentForAttackSlots = parentForAttackSlots;

        if (spawnUnitEventManager != null)
        {
            spawnUnitEventManager.OnPlayerSpawn += Spawn;
        }
    }

    private void Spawn(Player player)
    {
        foreach (var baseAttack in player.BaseAttacks)
        {
            var go = _container.InstantiatePrefab(_attackSlotProfab, _parentForAttackSlots);

            var attackSlot = go.GetComponent<AttackSlot>();

            attackSlot.ImageAttack.sprite = baseAttack.AttackData.Icon;
            attackSlot.BaseAttack = baseAttack;
        }
    }
}

