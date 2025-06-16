using UnityEngine;

public class ShotState : IState
{
    private SecurityGuard _securityGuard;
    public ShotState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }

    enum ShotSection
    {
        aim,
        shot,
        shotInterval,
        sum
    }

    private ShotSection shotSection;

    private Rigidbody2D secRigidBody2D;

    Vector3 shootDirection;
    float aimTime = 0.5f;
    float timer = 0;
    float reloadTime = 2;

    int MAXMAGAGINE = 12;
    int nowMagazine = 0;


    private int shotRate = 3;

    float shotIntevalTime = 0;
    private int shotNum = 0;
    public void Enter()
    {
        secRigidBody2D = _securityGuard.GetRigidbody2D();
    }

    public void Execute()
    {
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
                shootDirection = Quaternion.Euler(0, 0, _securityGuard.transform.eulerAngles.z) * Vector3.up;
                //GunFire();
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
                        
                        shotSection = ShotSection.aim;
                    }
                    shotSection = ShotSection.shot;
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