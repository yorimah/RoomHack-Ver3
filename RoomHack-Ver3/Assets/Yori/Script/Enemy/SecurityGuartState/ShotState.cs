using UnityEngine;

public class ShotState : IState
{
    private Enemy enemy;

    // GunData
    private GunData gundata;
    private float aimTime = 0.5f;
    private float timer;
    private int MaxMagazine;
    private int nowMagazine;
    private int shotRate;
    private float bulletSpeed;
    // ShotSection
    private float shotIntevalTime = 0;
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

    // Player情報
    private PlayerCheack playerCheack;
    public ShotState(Enemy _enemy)
    {
        enemy = _enemy;
        EnemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        // GunData初期化
        gundata = enemy.gundata;
        shotRate = gundata.rate;
        MaxMagazine = gundata.MaxMagazine;
        nowMagazine = MaxMagazine;
        shotIntevalTime = 1f / shotRate;

        bulletSpeed = gundata.bulletSpeed;
        bulletGeneratar = enemy.gameObject.GetComponent<BulletGeneratar>();

        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;
    }

    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
        nowMagazine = enemy.nowMagazine;
    }

    public void Execute()
    {
        playerCheack.RotationFoward(enemy.transform);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:

                EnemyRigidBody2D.velocity = Vector2.zero;
                timer += Time.deltaTime;
                if (aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                if (nowMagazine <= 0)
                {
                    enemy.ChangeState(Enemy.StateType.Reload);
                    return;
                }
                bulletGeneratar.GunFire(bulletSpeed, enemy.HitDamegeLayer);
                nowMagazine--;
                shotNum++;
                shotSection++;
                break;
            case ShotSection.shotInterval:
                timer += Time.deltaTime;
                if (shotIntevalTime <= timer)
                {
                    timer = 0;
                    if (shotNum >= shotRate)
                    {
                        shotNum = 0;
                        if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
                        {
                            shotSection = ShotSection.aim;
                        }
                        else
                        {
                            enemy.ChangeState(Enemy.StateType.Move);
                        }
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
        enemy.nowMagazine = nowMagazine;
    }
}