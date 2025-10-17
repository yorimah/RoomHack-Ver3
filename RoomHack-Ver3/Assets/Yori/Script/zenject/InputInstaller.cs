using Zenject;

public class InputInstaller : MonoInstaller<InputInstaller>
{
    public override void InstallBindings()
    {
        Container
         .Bind<IPlayerInput>()   // IPlayerInputが要求されたら
                .To<PlayerInput>() // PlayerInputを生成して注入する
                .AsCached();              // ただし、PlayerInputが生成済みなら使い回す
    }
}