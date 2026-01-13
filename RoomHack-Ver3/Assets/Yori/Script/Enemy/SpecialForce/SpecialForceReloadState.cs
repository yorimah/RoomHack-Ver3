using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpecialForceReloadState : IEnemyState
{
    private Enemy enemy;

    // Player情報
    private PlayerCheck playerCheck;

    private Rigidbody2D enemyRididBody;
    public SpecialForceReloadState(Enemy _enemy)
    {
        enemy = _enemy;
        // プレイヤー情報初期化
        playerCheck = enemy.playerCheck;

        enemyRididBody = enemy.GetComponent<Rigidbody2D>();
    }
    private float timer = 0;

    float nowHP;
    public void Enter()
    {
        nowHP = enemy.NowHitPoint;
        timer = 0;
        enemyRididBody = enemy.GetComponent<Rigidbody2D>();        
    }

    public void Execute()
    {
        enemyRididBody.linearVelocity = Vector2.zero;
        // ひだんしたら
        if (nowHP != enemy.NowHitPoint)
        {
            enemy.ChangeState(EnemyStateType.Move);
        }
        timer += GameTimer.Instance.GetScaledDeltaTime();
        if (timer >= 3)
        {
            enemy.NOWBULLET = enemy.MAXBULLET;
            enemy.ChangeState(EnemyStateType.Move);
        }
    }

    public void Exit()
    {

    }
}