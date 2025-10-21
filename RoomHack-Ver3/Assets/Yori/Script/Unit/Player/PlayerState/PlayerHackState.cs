using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerHackState : IPlayerState
{
    IPlayerInput playerInput;
    PlayerStateContoller playerStateContoller;

    public PlayerHackState(PlayerStateContoller _playerStateContoller, IPlayerInput _playerInput)
    {
        playerInput = _playerInput;
        playerStateContoller = _playerStateContoller;
    }

    public void Enter()
    {
        playerInput.ChangeState += ChangeState;
    }

    public async UniTask Execute()
    {
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
