using UnityEngine;
using Zenject;

public class WindowInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<WindowListHolder>().AsSingle();
        Container.BindInterfacesTo<PlayerStatus>().AsSingle();
    }
}