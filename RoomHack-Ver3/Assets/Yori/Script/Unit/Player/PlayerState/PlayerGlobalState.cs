using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerGlobalState : IPlayerState
{
    private PlayerShot playerShot;
    PlayerStateContoller playerStateContoller;
    public PlayerGlobalState(IGetGunData gunData, Material material, IPlayerInput playerInput,
        GameObject bulletPre, GameObject player, IHaveGun haveGun, PlayerStateContoller _playerStateContoller)
    {
        playerShot = new PlayerShot(gunData, material, player, bulletPre, playerInput, haveGun);
        playerStateContoller = _playerStateContoller;
    }

    public void Enter()
    {

    }

    public async UniTask Execute()
    {
        if (GameTimer.Instance.playTime <= 0)
        {
            playerStateContoller.ChangeState(PlayerStateType.Pause);
            return;
        }

        playerShot.Shot();
        await UniTask.Yield();
    }

    public void Exit()
    {

    }
}
