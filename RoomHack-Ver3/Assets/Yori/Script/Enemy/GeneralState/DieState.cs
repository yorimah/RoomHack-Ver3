using Cysharp.Threading.Tasks;
public class DieState : IState
{
    private Enemy enemy;
    PlayerSaveData data;
    public DieState(Enemy _enemy)
    {
        enemy = _enemy;
    }
    public void Enter()
    {
        SeManager.Instance.Play("EnemyDead");
        enemy.gameObject.SetActive(false);
    }

    public async UniTask Execute()
    {
        await UniTask.Yield();
    }

    public void Exit()
    {

    }
}