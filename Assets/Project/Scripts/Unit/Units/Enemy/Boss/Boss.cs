using System.Collections.Generic;
using Zenject;

public class Boss : Enemy
{
    public List<BossPhase> BossPhases;
    public BossPhase CurrentPhase { get; private set; }
    protected override IObjectPool ObjectPool { get; set; }
    protected override GlobalEventManager GlobalEventManager { get; set; }
    protected override IStateMachine StateMachine { get; set; }
    protected override UnitEventManager UnitEventManager { get; set; }

    private BossEventManager _bossEventManager;

    [Inject]
    public void Construct(BossEventManager bossEventManager, EnemyPool enemyPool,
        EnemyStateMachine enemyStateMachine, GlobalEventManager globalEventManager)
    {
        ObjectPool = enemyPool;
        StateMachine = enemyStateMachine;
        _bossEventManager = bossEventManager;
        UnitEventManager = bossEventManager;
        GlobalEventManager = globalEventManager;
    }

    public void ChargePhase(BossPhase newBossPhase)
    {
        CurrentPhase = newBossPhase;

        _bossEventManager.TriggerChangeBossPhase(newBossPhase);
    }
}
