using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpecialForceMoveState : IEnemyState
{
    private EnemyBase enemy;

    private Rigidbody2D enemyRigidBody;

    private float flipTimer = 0;
    private float flipInterval = 0.5f;

    private int forwardDir = 1;
    private int direction = 1;

    private PlayerCheck playerCheck;

    public SpecialForceMoveState(EnemyBase _enemy)
    {
        enemy = _enemy;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
        playerCheck = enemy.playerCheck;
    }
    public void Enter()
    {
        flipTimer = 0f;
    }

    public void Execute()
    {
        if (enemy.NOWBULLET > 0)
        {
            flipTimer += GameTimer.Instance.GetScaledDeltaTime();
            // 反転タイミングになったら前後どっちに反転するか決める。
            if (flipTimer >= flipInterval)
            {
                forwardDir = Random.value < 0.5f ? -1 : 1;
                flipTimer = 0f;
                // プレイヤーとの間に障害物があるかチェック ないならショット移動
                if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
                {
                    enemy.ChangeState(EnemyStateType.Shot);
                }
            }
        }
        else
        {
            forwardDir = 1;
            if (!playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(),enemy.PlayerPosition))
            {
                enemy.ChangeState(EnemyStateType.Reload);
            }
        }

        Vector2 nowPosition = enemy.transform.position;
        Vector2 center = enemy.PlayerPosition;
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
            enemyRigidBody.linearVelocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRigidBody.linearVelocity = moveDir.normalized * enemy.moveSpeed * GameTimer.Instance.GetCustomTimeScale();

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