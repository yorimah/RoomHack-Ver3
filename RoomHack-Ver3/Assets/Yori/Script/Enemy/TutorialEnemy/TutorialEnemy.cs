using UnityEngine;
using System.Collections.Generic;
public class TutorialEnemy : EnemyBase
{
    void Start()
    {
        playerCheck = new PlayerCheck();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new TutorialEnemyIdel(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        cantHackToolType = new List<ToolType> { };
        nowHackEvent = new List<ToolEventBase>();
    }
}

public class TutorialEnemyIdel : IEnemyState
{
    private EnemyBase enemy;
    public enum PatrolMoveEnum
    {
        moveStraight,
        turn,
    }
    PatrolMoveEnum patrolMoveEnum;

    private Rigidbody2D enemyRigidBody;

    private float checkDistance = 1;
    public TutorialEnemyIdel(EnemyBase _enemy)
    {
        enemy = _enemy;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
    }
    public void Enter()
    {
        patrolMoveEnum = PatrolMoveEnum.moveStraight;
    }

    public void Execute()
    {
        MoveTypeChange();
    }

    public void Exit()
    {
    }
    Vector2 targetDir;
    public void MoveTypeChange()
    {
        switch (patrolMoveEnum)
        {
            case PatrolMoveEnum.moveStraight:
                // 壁とぶつかったら左右どちらかに方向転換
                if (!Physics2D.Raycast(enemy.transform.position, enemy.transform.up, checkDistance, enemy.GetObstacleMask()))
                {
                    enemyRigidBody.linearVelocity = enemy.transform.up * enemy.moveSpeed * GameTimer.Instance.GetCustomTimeScale();
                }
                else
                {
                    Vector2 rightDir = enemy.transform.right;
                    Vector2 leftDir = -enemy.transform.right;
                    RaycastHit2D rightRangeRay = Physics2D.Raycast(enemy.transform.position, rightDir, 10, enemy.GetObstacleMask());
                    float rightRange = Vector2.Distance(enemy.transform.position, rightRangeRay.point);
                    RaycastHit2D leftRangeRay = Physics2D.Raycast(enemy.transform.position, leftDir, 10, enemy.GetObstacleMask());
                    float leftRange = Vector2.Distance(enemy.transform.position, leftRangeRay.point);
                    targetDir = rightRange >= leftRange ? rightDir : leftDir;
                    patrolMoveEnum = PatrolMoveEnum.turn;
                }
                break;
            case PatrolMoveEnum.turn:
                float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
                float current = enemy.transform.eulerAngles.z;
                float rotateSpeed = 180f;
                float newAngle = Mathf.MoveTowardsAngle(current, targetAngle, rotateSpeed * GameTimer.Instance.GetScaledDeltaTime());
                enemy.transform.rotation = Quaternion.Euler(0, 0, newAngle);
                float diff = Mathf.DeltaAngle(current, targetAngle);

                if (Mathf.Abs(diff) < 1f)
                {
                    patrolMoveEnum = PatrolMoveEnum.moveStraight;
                }
                break;
        }
    }
}
