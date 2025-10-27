using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerActionState : IPlayerState
{
    private PlayerMove playerMove;
    PlayerStateContoller playerStateContoller;

    IPlayerInput playerInput;
    public PlayerActionState(Rigidbody2D playerRigidBody, float moveSpeed, PlayerStateContoller _playerStateContoller,
        IPlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerStateContoller = _playerStateContoller;
        playerMove = new PlayerMove(playerRigidBody, moveSpeed, this.playerInput);
       
    }
    public void Enter()
    {
        playerInput.ChangeState += ChangeState;
    }
    public async UniTask Execute()
    {
        playerMove.playerMove();

        // 入力がないならハックステートへ
        if (playerInput.MoveValue() == Vector2.zero)
        {
            ChangeState();
        }
        await UniTask.Yield();
    }
    private void ChangeState()
    {
        playerStateContoller.ChangeState(PlayerStateType.Hack);
        GameTimer.Instance.SetHackModeTimeScale();
    }
    public void Exit()
    {
        playerInput.ChangeState -= ChangeState;
    }
}
