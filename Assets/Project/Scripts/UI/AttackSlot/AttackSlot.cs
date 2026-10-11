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

        InitializeCooldownSubscription().Forget();
    }

    private void StartRestoreAttackFill()
    {
        RestoreAttackFill(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid RestoreAttackFill(System.Threading.CancellationToken cancellationToken)
    {
        var cooldownDuration = BaseAttack.AttackData.Cooldown;
        var currentTime = cooldownDuration;

        BaseAttack.AttackLoop(cooldownDuration, cancellationToken).Forget();

        while (currentTime >= 0 && !cancellationToken.IsCancellationRequested)
        {
            _displayValueBar.Display(currentTime, cooldownDuration);

            bool isCancelled = await UniTask.DelayFrame(1, cancellationToken: cancellationToken).SuppressCancellationThrow();
            if (isCancelled) return;

            currentTime -= Time.deltaTime;
        }
    }

    private async UniTaskVoid InitializeCooldownSubscription()
    {
        bool isCancelled = await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow();
        if (isCancelled) return;

        if (BaseAttack != null)
        {
            BaseAttack.OnAttack += StartRestoreAttackFill;
        }
    }

    private void OnDestroy()
    {
        if (BaseAttack != null)
        {
            BaseAttack.OnAttack -= StartRestoreAttackFill;
        }
    }
}
