using Cysharp.Threading.Tasks;

public interface IPlayerState
{
    void Enter();
    UniTask Execute();
    void Exit();
}
