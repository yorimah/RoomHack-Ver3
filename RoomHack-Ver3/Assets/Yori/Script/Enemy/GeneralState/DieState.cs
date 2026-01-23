using Cysharp.Threading.Tasks;
public class DieState : IEnemyState
{
    private EnemyBase enemy;
    PlayerSaveData data;
    public DieState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }
    public void Enter()
    {
        SeManager.Instance.Play("EnemyDead");
        enemy.gameObject.SetActive(false);
    }

    public void Execute()
    {
    }

    public void Exit()
    {

    }
}