using System;


public class BossEventManager : EnemyEventManager
{
    public event Action<BossPhase> OnChangeBossPhase;

    public void TriggerChangeBossPhase(BossPhase newBossPhase) => OnChangeBossPhase?.Invoke(newBossPhase);
}

