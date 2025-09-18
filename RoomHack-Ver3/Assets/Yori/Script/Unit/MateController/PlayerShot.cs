using UnityEngine;

public class PlayerShot
{
    private UnitCore unitCore;


    enum ShotSection
    {
        shot,
        shotInterval

    }

    ShotSection shotSection;

    private float timer = 0;
    public PlayerShot(UnitCore _unitCore)
    {
        unitCore = _unitCore;
    }
    private void GunFire()
    {
        GameObject bulletGameObject = Object.Instantiate(unitCore.bulletPrefab, unitCore.transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.power = unitCore.stoppingPower;
        bulletCore.hitStop = 0.1f;
        bulletCore.HitDamegeLayer = unitCore.HitDamegeLayer;

        Vector3 shootDirection = Quaternion.Euler(0, 0, unitCore.transform.eulerAngles.z) * Vector3.up;
        bulletRigit.velocity = shootDirection * unitCore.bulletSpeed;
        bulletGameObject.transform.up = shootDirection;
    }
    public void Shot()
    {
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.shot:
                if (unitCore.NOWBULLET<=0)
                {
                    return;
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GunFire();
                    shotSection++;
                    unitCore.NOWBULLET--;
                }
                break;
            case ShotSection.shotInterval:
                timer += Time.deltaTime;
                if (unitCore.shotIntervalTime <= timer)
                {
                    shotSection = ShotSection.shot;
                    timer = 0;
                }
                break;
            default:
                break;
        }
    }
}