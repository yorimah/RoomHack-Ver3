using UnityEngine;

public class SpecialForceShotState : IState
{
    private Enemy enemy;

    // GunData

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

    private BulletGeneratar bulletGeneratar;

    private float timer;
    // Player情報
    private PlayerCheack playerCheack;
    public SpecialForceShotState(Enemy _enemy)
    {
        enemy = _enemy;
        EnemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        // GunData初期化

        bulletGeneratar = enemy.gameObject.GetComponent<BulletGeneratar>();

        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;
    }
    float nowHP;

    private float diffusionRate = 0;
    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
        nowHP = enemy.NowHP;
        diffusionRate = 0;
    }

    public void Execute()
    {
        if (nowHP != enemy.NowHP)
        {
            enemy.ChangeState(Enemy.StateType.Move);
        }
        playerCheack.RotationFoward(enemy.transform);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:

                EnemyRigidBody2D.velocity = Vector2.zero;
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (enemy.aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                if (enemy.NOWBULLET <= 0)
                {
                    enemy.ChangeState(Enemy.StateType.Move);
                    return;
                }
                diffusionRate += enemy.recoil;
                Mathf.Clamp(diffusionRate, enemy.minDiffusionRate, enemy.maxDiffusionRate);
                Debug.Log(diffusionRate);
                bulletGeneratar.GunFire(enemy.bulletSpeed, enemy.HitDamegeLayer, enemy.stoppingPower, diffusionRate);
                enemy.NOWBULLET--;
                shotNum++;
                shotSection++;
                break;
            case ShotSection.shotInterval:
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (enemy.shotIntervalTime <= timer)
                {
                    timer = 0;
                    if (shotNum >= enemy.shotRate)
                    {
                        shotNum = 0;
                        enemy.ChangeState(Enemy.StateType.Move);
                    }
                    else
                    {
                        shotSection = ShotSection.shot;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Exit()
    {
        timer = 0;
    }
}