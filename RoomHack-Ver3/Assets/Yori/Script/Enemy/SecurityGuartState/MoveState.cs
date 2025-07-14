using UnityEngine;

public class MoveState : IState
{
    private Enemy enemy;

    private Rigidbody2D enemyRididBody;

    private float flipTimer = 0;

    private float flipInterval = 1;

    private int direction = 1;

    private PlayerCheack playerCheack;
    public MoveState(Enemy _enemy)
    {
        enemy = _enemy;
        enemyRididBody = enemy.GetComponent<Rigidbody2D>();
        playerCheack = enemy.playerCheack;
    }

    public void Enter()
    {
        flipTimer = 0;

        flipInterval = 1;

        direction = Random.value < 0.5f ? -1 : 1;
    }

    public void Execute()
    {
        if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
        {
            enemy.ChangeState(Enemy.StateType.Shot);
        }
        Vector2 nowPosition = enemy.transform.position;
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }

        Vector2 center = UnitCore.Instance.transform.position;
        Vector2 dir = nowPosition - center;

        Vector2 emDir = new Vector2(-dir.y, dir.x);
        Vector2 nextPos = (nowPosition + (emDir * direction));

        // 障害物チェック
        Vector2 directionToNext = (nextPos - nowPosition).normalized;
        float checkDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(nowPosition, directionToNext, checkDistance, enemy.GetObstacleMask());
        //Debug.DrawRay(nowPosition, directionToNext * checkDistance, Color.blue);
        if (hit.collider != null)
        {
            direction *= -1;
            flipTimer = 0f;
            enemyRididBody.velocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRididBody.velocity = directionToNext.normalized * enemy.moveSpeed * GameTimer.Instance.customTimeScale;
    }

    public void Exit()
    {

    }
}

