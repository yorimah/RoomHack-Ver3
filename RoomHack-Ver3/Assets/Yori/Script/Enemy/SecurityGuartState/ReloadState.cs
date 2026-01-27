using UnityEngine;
using Cysharp.Threading.Tasks;
public class ReloadState : IEnemyState
{
    private EnemyBase enemy;
    public ReloadState(EnemyBase _enemy)
    {
        enemy = _enemy;        
    }

    private float timer = 0;
    public void Enter()
    {
        enemy.NOWBULLET = enemy.MAXBULLET;
        timer = 0;
    }

    public void Execute()
    {
        timer += GameTimer.Instance.GetScaledDeltaTime();
        if (timer >= enemy.gunData.ReloadTime)
        {
            enemy.ChangeState(EnemyStateType.Move);
            timer = 0;
        }
    }

    public void Exit()
    {
        enemy.NOWBULLET = enemy.gunData.MaxBullet;
    }
}