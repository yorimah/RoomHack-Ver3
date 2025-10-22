using Zenject;

public class InputInstaller : MonoInstaller<InputInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PlayerInput>().AsSingle();      // PlayerInputをシングルトンとして運用
        Container.BindInterfacesTo<PlayerStatus>().AsSingle();
    }
}