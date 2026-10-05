using UnityEngine;
using Zenject;

public class SpawnUnitFactory
{
    private readonly IInstantiator _container;
    private readonly SpawnUnitEventManager _spawnUnitEventManager;

    public SpawnUnitFactory(IInstantiator container, SpawnUnitEventManager spawnUnitEventManager)
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
        enemy.Health = new Health(enemyProfile.Data);

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

        var go2 = _container.InstantiatePrefab(playerProfile.HealthBarPrefab, parentForPlayerHealthBar);

        var player = go1.GetComponentInChildren<Player>();
        var healthBar = go2.GetComponent<HealthBar>();

        player.PlayerContext = go1;

        player.Health = new Health(playerProfile.Data, false);

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

