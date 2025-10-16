using UnityEngine;
using Cysharp.Threading.Tasks;
public class ReloadState : IState
{
    private Enemy enemy;
    public ReloadState(Enemy _enemy)
    {
        enemy = _enemy;        
    }

    private float timer = 0;
    public void Enter()
    {
        enemy.NOWBULLET = enemy.MAXBULLET;
        timer = 0;
    }

    public async UniTask Execute()
    {
        timer += GameTimer.Instance.ScaledDeltaTime;
        if (timer >= enemy.gunData.ReloadTime)
        {
            enemy.ChangeState(EnemyStateType.Move);
            timer = 0;
        }
        await UniTask.Yield();
    }

    public void Exit()
    {
        enemy.NOWBULLET = enemy.gunData.MaxBullet;
    }
}