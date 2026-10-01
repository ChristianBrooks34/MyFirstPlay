using UnityEngine;
using UnityEngine.UI;

public class SpawnUnitForUIFactory // ??
{
    public GameObject PlayerEmptySpawn(PlayerProfile playerProfile, Transform parentForPlayer, bool isDisplayWeapon)
    {
        if (playerProfile == null)
        {
            Debug.LogError("PlayerProfile is null in PlayerEmptySpawn!");
            return null;
        }

        if (parentForPlayer == null)
        {
            Debug.LogError("Parent transform is null in PlayerEmptySpawn! Check hierarchy or DI binding.");
            return null;
        }

        if (playerProfile.UnitPrefabForUI == null)
        {
            Debug.LogError("Player prefab is null! Assign prefab in inspector or DI.");
            return null;
        }

        var go1 = GameObject.Instantiate(playerProfile.UnitPrefabForUI, parentForPlayer);

        if (isDisplayWeapon)
        {
            var weapon = go1.GetComponentInChildren<Weapon>();

            var spriteRendererWeapon = weapon.GetComponentInChildren<SpriteRenderer>();

            GameObject.Destroy(weapon.GetComponentInChildren<SpriteRenderer>());

            var imageWeapon = weapon.gameObject.AddComponent<Image>();

            imageWeapon.sprite = spriteRendererWeapon.sprite;

            GameObject.Destroy(weapon.GetComponent<Weapon>());
            GameObject.Destroy(weapon.GetComponent<Animator>());
        }

        return go1;
    }
}
