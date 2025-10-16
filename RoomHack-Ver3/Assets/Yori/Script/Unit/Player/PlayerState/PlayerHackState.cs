using Cysharp.Threading.Tasks;

public class PlayerHackState : IState
{
    private PlayerInput playerInput;
    private PlayerStateContoller playerStateContoller;
    public PlayerHackState(PlayerInput _playerInput, PlayerStateContoller _playerStateContoller)
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
