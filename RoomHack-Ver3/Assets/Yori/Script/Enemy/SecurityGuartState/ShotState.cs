using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShotState : IEnemyState
{
    private Enemy enemy;
    // ShotSection
    private int shotNum = 0;
    enum ShotSection
    {
        aim,
        shot,
        shotInterval,
        sum
    }
    private ShotSection shotSection;

    private Rigidbody2D EnemyRigidBody2D;

    // 射撃用スクリプト
    private BulletGeneratar bulletGeneratar;

    // 汎用タイマー
    private float timer;
    // Player情報
    private PlayerCheack playerCheack;

    public ShotState(Enemy _enemy)
    {
        enemy = _enemy;
        EnemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        bulletGeneratar = enemy.gameObject.GetComponent<BulletGeneratar>();

        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;
    }

    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
    }

    public void Execute()
    {
        // プレイヤー方向に向く
        playerCheack.RotationFoward(enemy.transform,enemy.PlayerPosition);
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
                    EnemyRigidBody2D.linearVelocity = Vector2.zero;
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
                        shotNum++;
                        shotSection++;
                    }

                }
                break;
            case ShotSection.shotInterval:
                if (enemy.shotIntervalTime <= timer)
                {
                    timer = 0;
                    if (shotNum >= enemy.shotRate)
                    {
                        shotNum = 0;
                        // プレイヤーが射線上にいたら射撃予備動作へ
                        // いなかったら移動へ
                        if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(),enemy.PlayerPosition))
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
                        shotSection = ShotSection.shot;
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
    }
}