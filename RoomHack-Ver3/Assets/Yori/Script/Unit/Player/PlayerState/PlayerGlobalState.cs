using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerGlobalState : IPlayerState
{
    private PlayerShot playerShot;
    public PlayerGlobalState(IGunData gunData, Material material, IPlayerInput playerInput,  
        GameObject bulletPre, GameObject player,IHaveGun haveGun)
    {
        playerShot = new PlayerShot(gunData, material, player, bulletPre, playerInput,haveGun);
    }

    public void Enter()
    {
        
    }

    public async UniTask Execute()
    {
        playerShot.Shot();
        await UniTask.Yield();
    }

    public void Exit()
    {
        
    }
}
