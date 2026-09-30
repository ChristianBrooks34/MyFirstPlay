using Zenject;

public class WaveInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .BindInterfacesTo<WaveManager>()
            .AsCached();

        Container
            .Bind<WaveManager>()
            .AsCached();
    }
}