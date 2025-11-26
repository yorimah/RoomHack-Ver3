using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerGlobalState : IPlayerState
{
    private PlayerShot playerShot;
    private Rigidbody2D rigidbody2D;
    public PlayerGlobalState(IGetGunData gunData, Material material, IPlayerInput playerInput,  
        GameObject bulletPre, GameObject player,IHaveGun haveGun,Rigidbody2D _rigidbody2D)
    {
        playerShot = new PlayerShot(gunData, material, player, bulletPre, playerInput,haveGun);
        rigidbody2D = _rigidbody2D;
    }

    public void Enter()
    {
        
    }

    public async UniTask Execute()
    {
        playerShot.Shot();
        rigidbody2D.linearVelocity *= GameTimer.Instance.playTime;
        await UniTask.Yield();
    }

    public void Exit()
    {
        
    }
}
