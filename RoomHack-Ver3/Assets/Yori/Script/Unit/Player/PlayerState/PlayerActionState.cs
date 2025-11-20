using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerActionState : IPlayerState
{
    PlayerMove playerMove;
    PlayerStateContoller playerStateContoller;

    IPlayerInput playerInput;

    public PlayerActionState(Rigidbody2D playerRigidBody, IGetMoveSpeed getMoveSpeed,
        PlayerStateContoller _playerStateContoller, IPlayerInput _playerInput)
    {
        playerInput = _playerInput;
        playerStateContoller = _playerStateContoller;
        playerMove = new PlayerMove(playerRigidBody, getMoveSpeed, playerInput);

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
