using UnityEngine;

public class ShotState : IState
{
    private SecurityGuard _securityGuard;

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

    private Rigidbody2D secRigidBody2D;

    private BulletGeneratar bulletGeneratar;

    // Player情報
    private PlayerCheack playerCheack;
    public ShotState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
        secRigidBody2D = _securityGuard.GetComponent<Rigidbody2D>();

        // GunData初期化
        gundata = _securityGuard.gundata;
        shotRate = gundata.rate;
        MaxMagazine = gundata.MaxMagazine;
        nowMagazine = MaxMagazine;
        shotIntevalTime = 1f / shotRate;

        bulletSpeed = gundata.bulletSpeed;
        bulletGeneratar = _securityGuard.gameObject.GetComponent<BulletGeneratar>();

        // プレイヤー情報初期化
        playerCheack = _securityGuard.playerCheack;
    }

    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
        nowMagazine = _securityGuard.nowMagazine;
    }

    public void Execute()
    {
        playerCheack.RotationFoward(_securityGuard.transform);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:

                secRigidBody2D.velocity = Vector2.zero;
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
                    _securityGuard.ChangeState(SecurityGuard.StateType.Reload);
                    return;
                }
                bulletGeneratar.GunFire(bulletSpeed, _securityGuard.HitDamegeLayer);
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
                        if (playerCheack.PlayerRayHitCheack(_securityGuard.transform, _securityGuard.GetObstacleMask()))
                        {
                            shotSection = ShotSection.aim;
                        }
                        else
                        {
                            _securityGuard.ChangeState(SecurityGuard.StateType.Move);
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
        _securityGuard.nowMagazine = nowMagazine;
    }
}