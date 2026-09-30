using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttackSlot : MonoBehaviour
{
    public BaseAttack BaseAttack { get; set; }

    public Image ImageAttack;
    public Image ImageAttackCooldawnBar;
    public ValueBar AttackCooldawnBar;

    private DisplayValueBar _displayValueBar;

    [Inject]
    public void Construct()
    {
        var displayValueBar = new DisplayValueBar(ImageAttackCooldawnBar, AttackCooldawnBar);

        _displayValueBar = displayValueBar;

        Cooldawn().Forget();
    }

    private void StartRestoreAttackFill()
    {
        RestoreAttackFill().Forget();
    }

    private async UniTaskVoid RestoreAttackFill()
    {
        var currentTime = BaseAttack.AttackData.Cooldown;

        BaseAttack.AttackLoop(BaseAttack.AttackData.Cooldown).Forget();

        while (currentTime >= 0)
        {
            _displayValueBar.Display(currentTime, BaseAttack.AttackData.Cooldown);

            await UniTask.DelayFrame(1);

            currentTime -= Time.deltaTime;
        }
    }

    private async UniTaskVoid Cooldawn()
    {
        await UniTask.Delay(500);
        BaseAttack.OnAttack += StartRestoreAttackFill;
    }
}
