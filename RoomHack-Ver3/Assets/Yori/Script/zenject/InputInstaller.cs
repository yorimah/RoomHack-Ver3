using Zenject;
using UnityEngine;

public class InputInstaller : MonoInstaller<InputInstaller>
{
    public GameObject missilePre;
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PlayerInput>().AsSingle();      // PlayerInputをシングルトンとして運用
        Container.BindInterfacesTo<PlayerStatus>().AsSingle();
        Container.BindFactory<float, Vector2, IPosition,Vector2, Missile, Missile.Factory>()
                       .FromComponentInNewPrefab(missilePre);
        Container.BindInterfacesTo<GunDataList>().AsSingle();
        Container.BindInterfacesTo<EnemyList>().AsSingle();
        Container.BindInterfacesTo<ClearManager>().AsSingle();
    }
}