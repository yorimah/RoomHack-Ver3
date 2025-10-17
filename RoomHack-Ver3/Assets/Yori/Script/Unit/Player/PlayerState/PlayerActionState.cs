using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerActionState : IPlayerState
{
    private PlayerMove playerMove;
    private PlayerShot playerShot;
    PlayerStateContoller playerStateContoller;

    private PlayerInput playerInput;
    public PlayerActionState(Rigidbody2D playerRigidBody, GunData gunData, Material material, GameObject bulletPre,
        float moveSpeed, PlayerInput _playerInput, GameObject player, PlayerStateContoller _playerStateContoller)
    {
        playerInput = _playerInput;
        playerStateContoller = _playerStateContoller;
        playerMove = new PlayerMove(playerRigidBody, moveSpeed, playerInput);
        playerShot = new PlayerShot(gunData, material, bulletPre, player, playerInput);
    }
    public void Enter()
    {
        playerInput.ChangeState += ChangeState;
    }
    public async UniTask Execute()
    {
        playerMove.playerMove();
        playerShot.Shot();
        await UniTask.Yield();
    }
    private void ChangeState()
    {
        playerStateContoller.ChangeState(PlayerStateType.Hack);
    }
    public void Exit()
    {
        playerInput.ChangeState -= ChangeState;
    }
}
