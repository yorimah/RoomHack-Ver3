using UnityEngine;

public class SecurtyGuradClackState : IState
{
    private Enemy enemy;
    public SecurtyGuradClackState(Enemy _enemy)
    {
        enemy = _enemy;
    }
    public void Enter()
    {
        // 発射レートを半分にする
        enemy.shotRate = enemy.gundata.rate / 2;
        enemy.ChangeState(Enemy.StateType.Idle);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}