using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;

public class HealthDrop : Drop, IPickable
{
    private Player _player;
    private Game _game;
    private CancellationTokenSource _cts;
    private VisualEffectManager _visualEffectManager;

    [Inject]
    public void Construct(Game game, Player player, VisualEffectManager visualEffectManager)
    {
        _game = game;
        _player = player;
        _visualEffectManager = visualEffectManager;

        StartAnimation();
    }

    public bool CanBePicked()
    {
        return false;
    }

    public void OnPicked()
    {
        _player.Health.Add(DropProfile.BaseData.Count);

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
