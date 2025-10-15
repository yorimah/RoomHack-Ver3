using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerActionState : IState
{
    private PlayerMove playerMove;
    private PlayerShot playerShot;

    private PlayerInput playerInput;
    public PlayerActionState(Rigidbody2D playerRigidBody, GunData gunData, Material material, GameObject bulletPre, float moveSpeed, PlayerInput _playerInput,GameObject player)
    {
        playerMove = new PlayerMove(playerRigidBody, moveSpeed);
        playerShot = new PlayerShot(gunData, material, bulletPre, player);
        playerInput = _playerInput;
    }
    public void Enter()
    {
        Debug.Log("アクションState起動!");
    }
    public async UniTask Execute()
    {
        await UniTask.Yield();
        playerMove.playerMove(playerInput.MoveValue());
        playerShot.Shot();
    }

    public void Exit()
    {

    }
}
