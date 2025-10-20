using Cysharp.Threading.Tasks;
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
        await UniTask.Yield();
    }

    private void ChangeState()
    {
        playerStateContoller.ChangeState(PlayerStateType.Action);
    }

    public void Exit()
    {
        playerInput.ChangeState -= ChangeState;
    }
}
