using UnityEngine;
using Zenject;

public class SpawnUnitFactory
{
    private readonly DiContainer _container;
    private readonly SpawnUnitEventManager _spawnUnitEventManager;

    public SpawnUnitFactory(DiContainer container, SpawnUnitEventManager spawnUnitEventManager)
    {
        _container = container;
        _spawnUnitEventManager = spawnUnitEventManager;
    }

    public Enemy EnemySpawn(
        EnemyProfile enemyProfile, Vector2 position, Transform parentForEnemy,
        Transform parentForEnemyHealthBar, Player player)
    {
        enemyProfile.Data.Health.CurrentValue = enemyProfile.Data.Health.StartValue;

        var go = _container.InstantiatePrefab(enemyProfile.UnitPrefab, position, Quaternion.identity, parentForEnemy);
        var go2 = _container.InstantiatePrefab(enemyProfile.HealthBarPrefab, position, Quaternion.identity, parentForEnemyHealthBar);

        go.name = "Enemy" + Time.frameCount;

        var enemy = go.GetComponentInChildren<Enemy>();
        var healthBar = go2.GetComponentInChildren<HealthBar>();

        go2.transform.localScale = Vector3.one;

        healthBar.transform.localScale = Vector3.one * 150;
        healthBar.Initialize(enemy, enemy.Health);

        enemy.HealthBar = healthBar;

        enemy.EnemyContext = go;
        enemy.Target = player;

        healthBar.DisplayPoint = enemy.HealthBarDisplayPoint;

        enemy.Initialize(enemyProfile);

        _spawnUnitEventManager.TriggerEnemySpawn(enemy);

        return enemy;
    }


    public Player PlayerSpawn(
        PlayerProfile playerProfile, Vector2 position, Transform parentForPlayer,
        Transform parentForPlayerHealthBar)
    {
        playerProfile.Data.Exp = 0;

        var go1 = _container.InstantiatePrefab(playerProfile.UnitPrefab, position, Quaternion.identity, parentForPlayer);

        Debug.Log($"prefab == null: {playerProfile.HealthBarPrefab == null}");
        Debug.Log($"parentForPlayer == null: {parentForPlayer == null}");
        Debug.Log($"parentForPlayerHealthBar == null: {parentForPlayerHealthBar == null}");
        var go2 = _container.InstantiatePrefab(playerProfile.HealthBarPrefab, parentForPlayerHealthBar);

        var player = go1.GetComponentInChildren<Player>();
        var healthBar = go2.GetComponent<HealthBar>();

        player.PlayerContext = go1;

        healthBar.Initialize(player, player.Health);

        player.HealthBar = healthBar;

        player.Initialize(playerProfile);

        if (playerProfile.SwordChargeBarPrefab != null)
        {
            var go3 = _container.InstantiatePrefab(playerProfile.SwordChargeBarPrefab, parentForPlayerHealthBar);
            var swordChargeBar = go3.GetComponent<SwordChargeBar>();
            swordChargeBar.Initialize(player);
        }

        return player;
    }
}

