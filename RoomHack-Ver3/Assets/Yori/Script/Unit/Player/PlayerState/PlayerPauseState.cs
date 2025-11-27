using Cysharp.Threading.Tasks;
using UnityEngine;
public class PlayerPauseState : IPlayerState
{
    Rigidbody2D playerRigidBody;
    PlayerStateContoller playerStateContoller;
    Vector2 enterVelocity;
    public PlayerPauseState(PlayerStateContoller _playerStateContoller, Rigidbody2D _rigidbody2D)
    {
        playerStateContoller = _playerStateContoller;
        playerRigidBody = _rigidbody2D;
    }

    public void Enter()
    {
        if (playerRigidBody != null)
        {
            enterVelocity = playerRigidBody.linearVelocity;
            playerRigidBody.linearVelocity = Vector2.zero;
        }
    }

    public async UniTask Execute()
    {
        if (GameTimer.Instance.playTime > 0)
        {
            GameTimer.Instance.SetHackModeTimeScale();
            playerStateContoller.ChangeState(PlayerStateType.Hack);
        }
        await UniTask.Yield();
    }

    public void Exit()
    {
        if (playerRigidBody != null)
        {
            playerRigidBody.linearVelocity = enterVelocity;
        }
    }
}
