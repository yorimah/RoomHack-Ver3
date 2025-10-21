using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerGlobalState : IPlayerState
{
    private PlayerShot playerShot;
    public PlayerGlobalState(GunData gunData, Material material, IPlayerInput playerInput,  GameObject bulletPre, GameObject player)
    {
        playerShot = new PlayerShot(gunData, material, player, bulletPre, playerInput);
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
