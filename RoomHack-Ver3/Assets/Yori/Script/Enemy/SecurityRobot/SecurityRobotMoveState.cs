using System.Collections.Generic;
using UnityEngine;
public class SecurityRobotMoveState : IEnemyState
{
    private EnemyBase enemy;
    private PlayerCheck playerCheck;
    private Rigidbody2D enemyRigidBody;

    public IEnemyState currentState;

    private SecurityRobotMoveType securityRobotMoveType;
    private Dictionary<SecurityRobotMoveType, IEnemyState> moveTypeState;

    float moveTypeChangeTime = 1;
    float timer;

    private float checkDistance = 1;
    public SecurityRobotMoveState(EnemyBase _enemy)
    {
        enemy = _enemy;
        enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
        playerCheck = enemy.playerCheck;

        moveTypeState = new Dictionary<SecurityRobotMoveType, IEnemyState>()
        {
            { SecurityRobotMoveType.Straight , new SecurityRobotMoveStraightState(enemy)},
            { SecurityRobotMoveType.Circle , new SecurityRobotMoveCircleState(enemy)},
        };

        securityRobotMoveType = SecurityRobotMoveType.Straight;
        currentState = moveTypeState[securityRobotMoveType];
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        MoveChange();

        currentState?.Execute();
    }

    public void Exit()
    {

    }


    /// <summary>
    /// 遷移条件　一秒で直進と円周を切替、射線が通ったなら射撃へ
    /// </summary>
    public void MoveChange()
    {
        // 射線が通ったら射撃に遷移
        //if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
        //{
        //    enemy.ChangeState(EnemyStateType.Shot);
        //}
        if (securityRobotMoveType == SecurityRobotMoveType.Straight)
        {
            if (Physics2D.Raycast(enemy.transform.position, enemyRigidBody.linearVelocity, checkDistance, enemy.GetObstacleMask()))
            {
                Debug.Log("壁にぶつかりそうなので反転");
                MoveTypeChange(InversionEnum());
            }
        }
    }

    /// <summary>
    /// stateを反転して返す
    /// </summary>
    /// <returns></returns>
    public SecurityRobotMoveType InversionEnum()
    {
        if (securityRobotMoveType == SecurityRobotMoveType.Circle)
        {
            return SecurityRobotMoveType.Straight;
        }
        else
        {
            return SecurityRobotMoveType.Circle;
        }
    }
    public void MoveTypeChange(SecurityRobotMoveType movetype)
    {
        timer = 0;
        currentState.Exit();
        securityRobotMoveType = movetype;
        currentState = moveTypeState[securityRobotMoveType];
        currentState.Enter();
    }
}

public enum SecurityRobotMoveType
{
    none,
    Straight,
    Circle,
}
public class SecurityRobotMoveStraightState : IEnemyState
{
    float timer;
    public SecurityRobotMoveStraightState(EnemyBase enemy)
    {

    }
    public void Enter()
    {
        timer = 0;
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}

public class SecurityRobotMoveCircleState : IEnemyState
{
    public SecurityRobotMoveCircleState(EnemyBase enemy)
    {

    }
    public void Enter()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}