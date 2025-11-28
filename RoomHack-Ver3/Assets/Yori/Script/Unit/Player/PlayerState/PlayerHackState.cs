using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerHackState : IPlayerState
{
    IPlayerInput playerInput;
    PlayerStateContoller playerStateContoller;

    Rigidbody2D rigidbody2D;

    public PlayerHackState(PlayerStateContoller _playerStateContoller, IPlayerInput _playerInput, Rigidbody2D _rigidbody2D)
    {
        playerInput = _playerInput;
        playerStateContoller = _playerStateContoller;
        rigidbody2D = _rigidbody2D;
    }

    public void Enter()
    {
        playerInput.ChangeState += ChangeState;
    }

    public async UniTask Execute()
    {
        //rigidbody2D.linearVelocity *= GameTimer.Instance.GetScaledDeltaTime();
        if (playerInput.MoveValue() != Vector2.zero)
        {
            ChangeState();
        }
        await UniTask.Yield();
    }

    private void ChangeState()
    {
        playerStateContoller.ChangeState(PlayerStateType.Action);
        GameTimer.Instance.SetAcitionModeTimeScale();
    }

    public void Exit()
    {
        playerInput.ChangeState -= ChangeState;
    }
}
