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
        enemy.gameObject.SetActive(false);
    }

    public void Execute()
    {
    }

    public void Exit()
    {

    }
}