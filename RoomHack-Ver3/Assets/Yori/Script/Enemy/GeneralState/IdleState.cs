using UnityEngine;
using Cysharp.Threading.Tasks;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    public IdleState(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(enemy.transform.position);

        // 画面内判定　入ったらtrue
        bool isInside =
            viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1;
        if (isInside)
        {
            enemy.ChangeState(EnemyStateType.Move);
        }
    }

    public void Exit()
    {

    }
}

