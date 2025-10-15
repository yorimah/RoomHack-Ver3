using Cysharp.Threading.Tasks;

public interface IState
{
    void Enter();
    UniTask  Execute();
    void Exit();
}