using UnityEngine;


public class BossController
{
    private Boss _boss;
    private BossEventManager _bossEventManager;
    private LayerMask _damageableLayers;

    public BossController(Boss enemy, BossEventManager bossEventManager, LayerMask damageableLayers)
    {
        _boss = enemy;
        _bossEventManager = bossEventManager;
        _damageableLayers = damageableLayers;

        _boss.TriggerChecker.TriggerStay += OnTriggerStay;
        _boss.TriggerChecker.TriggerExit += OnTriggerExit;

        _bossEventManager.OnDamage += (x) => TrySwitchToNextPhase();
        _bossEventManager.OnChangeBossPhase += TryChargeAttack;

        SetStartPhase();
    }

    private void TryChargeAttack(BossPhase obj)
    {

    }

    private void TrySwitchToNextPhase()
    {
        if (_boss == null || _boss.CurrentPhase == null || _boss.BossPhases == null)
        {
            Debug.LogWarning("Boss or phase data is not initialized!");
            return;
        }

        float currentHealthPercent = _boss.GetHealthInProcent();

        float currentThreshold = _boss.CurrentPhase.HealthThreshold;

        if (currentHealthPercent >= currentThreshold) return;

        BossPhase nextPhase = null;
        float bestThreshold = float.MinValue;

        foreach (var phase in _boss.BossPhases)
        {
            if (phase.HealthThreshold < currentThreshold && phase.HealthThreshold > bestThreshold)
            {
                nextPhase = phase;
                bestThreshold = phase.HealthThreshold;
            }
        }

        if (nextPhase != null && nextPhase != _boss.CurrentPhase)
        {
            _boss.ChargePhase(nextPhase);
        }
    }

    private void SetStartPhase()
    {
        BossPhase newBossPhase = null;

        foreach (var bossPhase in _boss.BossPhases)
        {
            if (newBossPhase == null)
            {
                newBossPhase = bossPhase;
                continue;
            }

            if (newBossPhase.HealthThreshold < bossPhase.HealthThreshold)
            {
                newBossPhase = bossPhase;
            }
        }

        _boss.ChargePhase(newBossPhase);
    }

    private void OnTriggerStay(GameObject gameObject)
    {
        if (IsInDamageableLayer(gameObject.layer))
        {
            _boss.CanAttack = true;
            _bossEventManager.TriggerStartedAttack(gameObject);
        }
    }

    private void OnTriggerExit(GameObject gameObject)
    {
        if (IsInDamageableLayer(gameObject.layer))
        {
            _boss.CanAttack = false;
        }
    }

    private bool IsInDamageableLayer(int layer)
    {
        int layerAsBit = 1 << layer;

        return (_damageableLayers.value & layerAsBit) != 0;
    }
}

