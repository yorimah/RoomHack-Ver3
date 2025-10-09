using UnityEngine;

public class TurretGunShotState : IState
{
    private Enemy enemy;
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

    // 射撃拡散率
    private float diffusionRate;
    public TurretGunShotState(Enemy _enemy)
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
        //プレイヤー方向に向く
        playerCheack.RotationFoward(enemy.transform);
        //発射レートを設定しその後、発射秒数を決定する。
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
                    EnemyRigidBody2D.velocity = Vector2.zero;
                    timer += GameTimer.Instance.ScaledDeltaTime;
                }
                break;
            case ShotSection.shot:
                if (enemy.shotIntervalTime >= 100)
                {
                    shotSection++;
                }
                else
                {
                    // 拡散率加算
                    diffusionRate += enemy.recoil;
                    // 拡散率を固定、下限enemy.minDiffusionRate、上限 enemy.maxDiffusionRate
                    Mathf.Clamp(diffusionRate, enemy.minDiffusionRate, enemy.maxDiffusionRate);
                    // 射撃
                    bulletGeneratar.GunFire(enemy.bulletSpeed, enemy.HitDamegeLayer, enemy.stoppingPower, diffusionRate);

                    enemy.NOWBULLET--;
                    shotSection++;
                }

                break;
            case ShotSection.shotInterval:
                if (enemy.shotIntervalTime <= timer)
                {
                    // プレイヤーが射線上にいたら射撃へ
                    // いなかったら移動へ
                    if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
                    {
                        timer = 0;
                        shotSection = ShotSection.shot;
                    }
                    else
                    {
                        timer = 0;
                        enemy.ChangeState(StateType.Move);
                    }
                }
                else
                {
                    timer += GameTimer.Instance.ScaledDeltaTime;
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