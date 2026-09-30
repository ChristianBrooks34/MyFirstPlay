using System;

public class SpawnUnitEventManager
{
    public event Action<Player> OnPlayerSpawn;
    public event Action<Enemy> OnEnemySpawn;
    public event Action<Unit> OnUnitSpawn;

    public void TriggerPlayerSpawn(Player player)
    {
        OnPlayerSpawn?.Invoke(player);
    }

    public void TriggerEnemySpawn(Enemy enemy)
    {
        OnEnemySpawn?.Invoke(enemy);
    }

    public void TriggerUnitSpawn(Unit unit)
    {
        OnUnitSpawn?.Invoke(unit);
    }
}
