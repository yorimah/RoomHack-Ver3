using UnityEngine;

public class DroneMoveState : IState
{
    private PlayerCheack playerCheack;
    private Enemy enemy;

    private float flipTimer = 0;

    private float flipInterval = 1;

    private int direction = 1;
    private float moveDire;
    private Rigidbody2D enemyRigidBody2D;
    public DroneMoveState(Enemy _enemy)
    {
        enemy = _enemy;
        playerCheack = enemy.playerCheack;
        enemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();
    }
    public void Enter()
    {
        moveDire = Random.value < 0.5f ? -1 : 1;
        direction = Random.value < 0.5f ? -1 : 1;
    }

    public void Execute()
    {
        playerCheack.RotationFoward(enemy.transform);
        // レイを飛ばして射線が通たらショットに遷移
        if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
        {
            enemy.ChangeState(Enemy.StateType.Shot);
        }
        Vector2 nowPosition = enemy.transform.position;
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            moveDire = Random.value < 0.5f ? -1 : 1;
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }
        Vector2 moveXY = moveDire < 0.5f ? new Vector2(1, 0) : new Vector2(0, 1);

        Vector2 emDir = moveXY;
        Vector2 nextPos = (nowPosition + (emDir * direction));

        // 障害物チェック
        Vector2 directionToNext = (nextPos - nowPosition).normalized;
        float checkDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(nowPosition, directionToNext, checkDistance, enemy.GetObstacleMask());
        //Debug.DrawRay(nowPosition, directionToNext * checkDistance, Color.blue);
        if (hit.collider != null)
        {
            direction *= -1;
            moveDire = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
            enemyRigidBody2D.velocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRigidBody2D.velocity = directionToNext.normalized * enemy.moveSpeed * GameTimer.Instance.ScaledDeltaTime;
    }

    public void Exit()
    {

    }
}