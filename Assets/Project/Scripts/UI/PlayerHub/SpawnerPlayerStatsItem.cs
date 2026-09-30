using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayerStatsItem
{
    private readonly GameObject _stateItemPrefab;
    private readonly Transform _parentForStateItem;

    public SpawnerPlayerStatsItem(GameObject stateItemPrefab, Transform parentForStateItem)
    {
        _stateItemPrefab = stateItemPrefab;
        _parentForStateItem = parentForStateItem;
    }

    public List<PlayerStateItem> Spawn(PlayerProfile playerProfile, int count)
    {
        List<PlayerStateItem> playerStateItems = new List<PlayerStateItem>();

        for (int i = 0; i < count; i++)
        {
            var go = GameObject.Instantiate(_stateItemPrefab, _parentForStateItem);

            playerStateItems.Add(go.GetComponent<PlayerStateItem>());
        }

        return playerStateItems;
    }
}
