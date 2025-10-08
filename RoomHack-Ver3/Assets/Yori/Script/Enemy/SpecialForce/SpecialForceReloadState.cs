using UnityEngine;

public class SpecialForceReloadState : IState
{
    private Enemy enemy;

    // Player情報
    private PlayerCheack playerCheack;

    private Rigidbody2D enemyRididBody;
    public SpecialForceReloadState(Enemy _enemy)
    {
        enemy = _enemy;
        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;

        enemyRididBody = enemy.GetComponent<Rigidbody2D>();
    }
    private float timer = 0;

    float nowHP;
    public void Enter()
    {
        nowHP = enemy.NowHP;
        timer = 0;
        enemyRididBody = enemy.GetComponent<Rigidbody2D>();        
    }

    public void Execute()
    {
        enemyRididBody.velocity = Vector2.zero;
        // ひだんしたら
        if (nowHP != enemy.NowHP)
        {
            enemy.ChangeState(StateType.Move);
        }
        timer += GameTimer.Instance.ScaledDeltaTime;
        if (timer >= 3)
        {
            enemy.NOWBULLET = enemy.MAXBULLET;
            enemy.ChangeState(StateType.Move);
        }
    }

    public void Exit()
    {

    }
}