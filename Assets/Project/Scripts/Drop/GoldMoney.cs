using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

public class GoldMoney : Drop, IPickable
{
    private Wallet _wallet;
    private CancellationTokenSource _cts;
    private VisualEffectManager _visualEffectManager;

    [Inject]
    public void Construct(Wallet wallet, VisualEffectManager visualEffectManager)
    {
        _wallet = wallet;
        _visualEffectManager = visualEffectManager;

        StartAnimation();
    }

    public bool CanBePicked()
    {
        return false;
    }

    public void OnPicked()
    {
        _wallet.Add(DropProfile.BaseData.Count);

        Delete();
    }

    private void StartAnimation()
    {
        _cts = new CancellationTokenSource();

        _visualEffectManager.WaveVectical(_cts.Token, transform).Forget();
        _visualEffectManager.PulsateScale(_cts.Token, transform).Forget();
    }


    private void OnDestroy()
    {
        Delete();
    }

    private void Delete()
    {
        if (_isDead) return;

        _isDead = true;

        _cts.Cancel();
        _cts.Dispose();

        Destroy(gameObject);
    }
}
