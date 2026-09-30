using UnityEngine;

public class EnemyController
{
    private Enemy _enemy;
    private EnemyEventManager _enemyEventManager;
    private LayerMask _damageableLayers;

    public EnemyController(Enemy enemy, EnemyEventManager enemyEventManager, LayerMask damageableLayers)
    {
        _enemy = enemy;
        _enemyEventManager = enemyEventManager;
        _damageableLayers = damageableLayers;

        _enemy.TriggerChecker.TriggerStay += OnTriggerStay;
        _enemy.TriggerChecker.TriggerExit += OnTriggerExit;
    }

    private void OnTriggerStay(GameObject gameObject)
    {
        if (IsInDamageableLayer(gameObject.layer))
        {
            _enemy.CanAttack = true;
            _enemyEventManager.TriggerStartedAttack(gameObject);
        }
    }

    private void OnTriggerExit(GameObject gameObject)
    {
        if (IsInDamageableLayer(gameObject.layer))
        {
            _enemy.CanAttack = false;
        }
    }

    private bool IsInDamageableLayer(int layer)
    {
        int layerAsBit = 1 << layer;

        return (_damageableLayers.value & layerAsBit) != 0;
    }
}
