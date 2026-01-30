using UnityEngine;
using Zenject;

public class WindowInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<WindowListHolder>().AsSingle();
        Container.BindInterfacesTo<PlayerStatus>().AsSingle();
        Container.BindInterfacesTo<EnemyList>().AsSingle();
        Container.BindInterfacesTo<ClearManager>().AsSingle();
        Container.BindInterfacesTo<GunDataList>().AsSingle();
        Container.BindInterfacesTo<GameTimeHolder>().AsSingle();
    }
}