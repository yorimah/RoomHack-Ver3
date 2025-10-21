using Zenject;

public class PlayerInfoInstaller : MonoInstaller<PlayerInfoInstaller>
{
    public override void InstallBindings()
    {
        
        Container
            .Bind<IReadOnlyMoveSpeed>()
            .To<PlayerStatus>()
            .AsCached();        
        Container.Bind<IUseableRam>()
            .To<PlayerStatus>()
            .AsCached();
        Container.Bind<IDeckList>()
           .To<PlayerStatus>()
           .AsCached();
    }
}