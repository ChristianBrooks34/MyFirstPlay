using Zenject;

public class AnimatorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindAnimator();
    }

    private void BindAnimator()
    {
        Container
            .Bind<WeaponUpDownAnimator>()
            .AsSingle();

        Container
            .Bind<PlayerAnimator>()
            .AsSingle();
    }
}
