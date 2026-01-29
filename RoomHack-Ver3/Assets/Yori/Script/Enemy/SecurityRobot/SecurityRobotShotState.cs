using System.Collections.Generic;
using UnityEngine;

public class SecurityRobotShotState : IEnemyState
{
    private EnemyBase enemy;

    private PlayerCheck playerCheck;

    enum ShotSection
    {
        aim,
        shot,
        shotInterval,
        sum
    }
    ShotSection shotSection;

    float timer = 0;

    BulletGeneratar bulletGeneratar;

    public IEnemyState currentState;

    private SecurityRobotMoveType securityRobotMoveType;
    private Dictionary<SecurityRobotMoveType, IEnemyState> moveTypeState;
    public SecurityRobotShotState(EnemyBase _enemy)
    {
        enemy = _enemy;
        // プレイヤー情報初期化
        playerCheck = enemy.playerCheck;

        bulletGeneratar = enemy.gameObject.GetComponent<BulletGeneratar>();

        moveTypeState = new Dictionary<SecurityRobotMoveType, IEnemyState>()
        {
            { SecurityRobotMoveType.Straight , new SecurityRobotMoveStraightState(enemy)},
            { SecurityRobotMoveType.Circle , new SecurityRobotMoveCircleState(enemy)},
        };

        securityRobotMoveType = SecurityRobotMoveType.Straight;
        currentState = moveTypeState[securityRobotMoveType];
    }
    float initMoveSpeed;
    public void Enter()
    {
        initMoveSpeed = enemy.moveSpeed;
        enemy.moveSpeed /= 2;
    }

    public void Execute()
    {
        MoveChange();

        currentState?.Execute();
        // プレイヤー方向に向く
        playerCheck.RotationFoward(enemy.transform, enemy.PlayerPosition);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:
                if (enemy.aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                else
                {
                    timer += GameTimer.Instance.GetScaledDeltaTime();
                }
                break;
            case ShotSection.shot:
                if (enemy.NOWBULLET <= 0)
                {
                    enemy.ChangeState(EnemyStateType.Reload);
                    return;
                }
                else
                {
                    if (enemy.shotIntervalTime >= 100)
                    {
                        shotSection++;
                    }
                    else
                    {
                        // 拡散率加算
                        enemy.diffusionRate += enemy.recoil;
                        // 拡散率を固定、下限enemy.minDiffusionRate、上限 enemy.maxDiffusionRate
                        Mathf.Clamp(enemy.diffusionRate, enemy.minDiffusionRate, enemy.maxDiffusionRate);
                        // 射撃
                        bulletGeneratar.GunFire(enemy.bulletSpeed, enemy.hitDamegeLayer, enemy.stoppingPower, enemy.diffusionRate);

                        enemy.NOWBULLET--;
                        shotSection++;
                    }

                }
                break;
            case ShotSection.shotInterval:
                if (enemy.shotIntervalTime <= timer)
                {
                    // プレイヤーが射線上にいたら射撃予備動作へ
                    // いなかったら移動へ
                    if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
                    {
                        shotSection = ShotSection.aim;
                    }
                    else
                    {
                        enemy.ChangeState(EnemyStateType.Move);
                    }
                }
                else
                {
                    timer += GameTimer.Instance.GetScaledDeltaTime();
                }
                break;
            default:
                break;
        }
    }

    public void Exit()
    {
        enemy.moveSpeed = initMoveSpeed;
    }

    public void MoveChange()
    {
        // 射線が通ったら射撃に遷移
        if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
        {
            enemy.ChangeState(EnemyStateType.Shot);
        }
        if (Physics2D.Raycast(enemy.transform.position, enemy.rigidbody.linearVelocity, 1, enemy.GetObstacleMask()))
        {
            MoveTypeChange(InversionEnum());
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
        currentState.Exit();
        securityRobotMoveType = movetype;
        currentState = moveTypeState[securityRobotMoveType];
        currentState.Enter();
    }
}