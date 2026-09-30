using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DroneStriker : Enemy
{
    [SerializeField] private float _timeScaleDownAnim;

    private bool _isCancelScaleDownAnim;
    protected override IObjectPool ObjectPool { get; set; }
    protected override GlobalEventManager GlobalEventManager { get; set; }
    protected override IStateMachine StateMachine { get; set; }
    protected override UnitEventManager UnitEventManager { get; set; }

    private float _startScale;

    [Inject]
    public void Construct(UnitEventManager enemyEventManager, EnemyPool enemyPool,
        EnemyStateMachine enemyStateMachine, GlobalEventManager globalEventManager)
    {
        ObjectPool = enemyPool;
        StateMachine = enemyStateMachine;
        UnitEventManager = enemyEventManager;
        GlobalEventManager = globalEventManager;

        _startScale = Mathf.Abs(transform.localScale.x);
    }

    public override void RecycleDeactivate()
    {
        base.RecycleDeactivate();

        transform.localScale = Vector3.one * _startScale;
    }

    public override void Dead()
    {
        ScaleDownAnim().Forget();

        DeadCooldown().Forget();
    }

    private async UniTaskVoid ScaleDownAnim()
    {
        var currentTime = 0f;

        var coeficent = 1 / _timeScaleDownAnim;

        while (currentTime < _timeScaleDownAnim)
        {
            await UniTask.DelayFrame(1);

            currentTime += Time.deltaTime;

            var newScale = Mathf.Lerp(_startScale, 0.2f, currentTime * coeficent);

            transform.localScale = Vector3.one * newScale;
        }

        _isCancelScaleDownAnim = true;
    }

    private async UniTaskVoid DeadCooldown()
    {
        while (true)
        {
            if (_isCancelScaleDownAnim)
            {
                if (StateMachine.IsCurrentState(EnemyState.Dead) || IsDead) return;
                IsDead = true;

                DisableComponents();

                GlobalEventManager.TriggerDeadEnemy(this);
                UnitEventManager.TriggerDead(this);

                DeadRoutine(
                    () => ObjectPool.ReturnObject(gameObject),
                    UnitProfile.BaseData.DeadCooldown)
                    .Forget();

                break;
            }

            await UniTask.Delay(250);
        }
    }
}
