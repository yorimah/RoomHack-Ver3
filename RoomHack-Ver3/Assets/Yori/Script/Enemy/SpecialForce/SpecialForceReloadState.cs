using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpecialForceReloadState : IEnemyState
{
    private EnemyBase enemy;

    // Player情報
    private PlayerCheck playerCheck;

    private Rigidbody2D enemyRigidBody;
    public SpecialForceReloadState(EnemyBase _enemy)
    {
        enemy = _enemy;
        // プレイヤー情報初期化
        playerCheck = enemy.playerCheck;

        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
    }
    private float timer = 0;

    float nowHP;
    public void Enter()
    {
        nowHP = enemy.NowHitPoint;
        timer = 0;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();        
    }

    public void Execute()
    {
        enemyRigidBody.linearVelocity = Vector2.zero;
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