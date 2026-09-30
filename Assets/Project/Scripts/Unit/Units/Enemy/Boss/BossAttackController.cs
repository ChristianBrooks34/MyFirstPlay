using Cysharp.Threading.Tasks;


public class BossAttackController
{
    private readonly Boss _boss;

    public BossAttackController(Boss boss)
    {
        _boss = boss;

        Attack().Forget();
    }

    private async UniTaskVoid Attack()
    {
        while (_boss != null && !_boss.IsDead)
        {
            foreach (var attack in _boss.CurrentPhase.Attacks)
            {
                attack.Attack();

                await UniTask.Delay(_boss.CurrentPhase.AttackInterval);
            }
            await UniTask.DelayFrame(1);
        }
    }
}
