using UnityEngine;

public class SpecialForceMoveState : IState
{
    private Enemy enemy;

    private Rigidbody2D enemyRididBody;

    private float flipTimer = 0;
    private float flipInterval = 0.5f;

    private int forwardDir = 1;
    private int direction = 1;

    private PlayerCheack playerCheack;

    public SpecialForceMoveState(Enemy _enemy)
    {
        enemy = _enemy;
        enemyRididBody = enemy.GetComponent<Rigidbody2D>();
        playerCheack = enemy.playerCheack;
    }
    public void Enter()
    {
        flipTimer = 0f;
    }

    public void Execute()
    {
        if (enemy.NOWBULLET > 0)
        {
            flipTimer += GameTimer.Instance.ScaledDeltaTime;
            // 反転タイミングになったら前後どっちに反転するか決める。
            if (flipTimer >= flipInterval)
            {
                forwardDir = Random.value < 0.5f ? -1 : 1;
                flipTimer = 0f;
                // プレイヤーとの間に障害物があるかチェック ないならショット移動
                if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
                {
                    enemy.ChangeState(Enemy.StateType.Shot);
                }
            }
        }
        else
        {
            forwardDir = 1;
            if (!playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
            {
                enemy.ChangeState(Enemy.StateType.Reload);
            }
        }

        Vector2 nowPosition = enemy.transform.position;
        Vector2 center = UnitCore.Instance.transform.position;
        Vector2 toEnemy = nowPosition - center;
        Vector2 radialDir = toEnemy.normalized;

        // 横移動
        Vector2 tangentDir = new Vector2(-radialDir.y, radialDir.x) * direction;

        // 縦移動
        Vector2 forwardMove = radialDir * forwardDir;

        // 方向合成
        Vector2 moveDir = (tangentDir + forwardMove * 2).normalized;
        // 障害物チェック
        float checkDistance = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(nowPosition, moveDir, checkDistance, enemy.GetObstacleMask());
        //Debug.DrawRay(nowPosition, directionToNext * checkDistance, Color.blue);
        if (hit.collider != null)
        {
            direction *= -1;
            forwardDir *= -1;
            flipTimer = 0f;
            enemyRididBody.velocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRididBody.velocity = moveDir.normalized * enemy.moveSpeed * GameTimer.Instance.customTimeScale;

        enemy.transform.rotation = MoveForwadRotation(nowPosition + moveDir);
    }

    public void Exit()
    {

    }

    public Quaternion MoveForwadRotation(Vector3 _nextPos)
    {
        Vector3 nextPos = _nextPos;
        Vector2 direction = nextPos - enemy.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        return targetRotation;
    }
}