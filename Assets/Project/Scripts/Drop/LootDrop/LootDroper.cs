using UnityEngine;
using Zenject;

public class LootDroper
{
    private readonly UnitEventManager _unitEventManager;
    private readonly DropContainer _dropContainer;
    private readonly DiContainer _diContainer;

    public LootDroper(DiContainer diContainer, UnitEventManager unitEventManager, DropContainer dropContainer)
    {
        _diContainer = diContainer;
        _dropContainer = dropContainer;
        _unitEventManager = unitEventManager;

        _unitEventManager.OnDeadUnit += Drop;
    }

    public void Drop(Unit unit)
    {
        var dropItem = GetRandomDrop(unit);

        if (dropItem == null) return;
        if (dropItem.DropProfile.DropPrefab == null) return;

        var go = _diContainer.InstantiatePrefab(
            dropItem.DropProfile.DropPrefab,
            unit.PointDropSpawn.position,
            Quaternion.identity,
            null);

        var drop = go.GetComponent<Drop>();

        drop.DropProfile = dropItem.DropProfile;

        drop.DropProfile.BaseData.Count = Random.Range(dropItem.MinDropCount, dropItem.MaxDropCount);

        _dropContainer.Add(go);
    }

    private DropItem GetRandomDrop(Unit unit)
    {
        var totalWeight = 0f;

        unit.UnitProfile.BaseData.DropItems
            .ForEach(x => totalWeight += x.DropRage);

        if (totalWeight <= 0) return null;

        float randomValue = Random.Range(0, totalWeight);

        var camulativeWeight = 0f;

        foreach (var e in unit.UnitProfile.BaseData.DropItems)
        {
            camulativeWeight += e.DropRage;

            if (randomValue < camulativeWeight)
            {
                return e;
            }
        }

        return null;
    }
}
