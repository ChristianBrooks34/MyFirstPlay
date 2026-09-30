using Zenject;

public class Zombie : Enemy
{
    protected override IObjectPool ObjectPool { get; set; }
    protected override GlobalEventManager GlobalEventManager { get; set; }
    protected override IStateMachine StateMachine { get; set; }
    protected override UnitEventManager UnitEventManager { get; set; }

    [Inject]
    public void Construct(UnitEventManager enemyEventManager, EnemyPool enemyPool,
        EnemyStateMachine enemyStateMachine, GlobalEventManager globalEventManager)
    {
        ObjectPool = enemyPool;
        StateMachine = enemyStateMachine;
        UnitEventManager = enemyEventManager;
        GlobalEventManager = globalEventManager;
    }
}
