using Cysharp.Threading.Tasks;
using Zenject;
public class PlayerHackState : IPlayerState
{
    private IPlayerInput playerInput;
    private PlayerStateContoller playerStateContoller;
    public PlayerHackState( PlayerStateContoller _playerStateContoller,IPlayerInput iPlayerInput)
    {
        playerInput = iPlayerInput;
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
