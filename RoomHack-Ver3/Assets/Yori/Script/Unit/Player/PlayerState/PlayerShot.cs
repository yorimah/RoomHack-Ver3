using UnityEngine;

public class PlayerShot
{
    private GameObject player;
    enum ShotSection
    {
        shot,
        shotInterval,
        Reload,
    }

    Mesh mesh;
    ShotSection shotSection;

    private float diffusionRate;

    private GameObject shotRange;

    // 長さ
    private float viewDistance = 3f;
    // 分割数
    private int segment = 20;

    GunData gunData;

    private int hitDamageLayer = 1;
    private Material shotRanageMaterial;
    private GameObject bulletPre;

    IPlayerInput playerInput;

    IHaveGun haveGun;

    Vector3[] vertices;
    int[] triangles;

    IGetGunData getGunData;
    public PlayerShot(IGetGunData _getGunData, Material _shotRanageMaterial, GameObject _bulletPre,
        GameObject _player, IPlayerInput _playerInput, IHaveGun _haveGun)
    {
        player = _player;
        playerInput = _playerInput;
        haveGun = _haveGun;
        getGunData = _getGunData;
        gunData = getGunData.GetGunData(haveGun.GunName);
        haveGun.BulletSet(gunData.MaxBullet);
        shotRanageMaterial = _shotRanageMaterial;
        shotRange = new GameObject(player.gameObject.name + "shotRangge");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        shotRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        shotRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = shotRange.GetComponent<MeshRenderer>();

        mr.material = new Material(shotRanageMaterial);
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色(仮)
        mr.sortingOrder = 10;

        bulletPre = _bulletPre;

        vertices = new Vector3[segment + 2];
        triangles = new int[segment * 3];
    }

    private void GunFire()
    {
        haveGun.BulletUse();
        diffusionRate += gunData.Recoil;

        for (int i = 0; i < gunData.ShotBulletNum; i++)
        {
            float rand = Random.Range(-diffusionRate, diffusionRate);
            Vector2 shotDirection = Quaternion.Euler(0, 0, player.transform.eulerAngles.z + rand) * Vector3.up;
            GameObject bulletGameObject = Object.Instantiate(bulletPre, (Vector2)player.transform.position + shotDirection.normalized * 0.5f,
                Quaternion.identity);

            Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

            BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

            bulletCore.stoppingPower = gunData.Power;
            bulletCore.hitStopTime = 0.1f;
            bulletCore.hitDamegeLayer = hitDamageLayer;
            bulletRigit.linearVelocity = shotDirection * gunData.BulletSpeed;
            bulletGameObject.transform.up = shotDirection;
        }
    }


    public void Shot()
    {
        if (player == null)
        {
            return;
        }

        // マウスの方向に向く
        PlayerRotation();

        // 撃つ向きとブレのメッシュ表示
        ShotRangeView();

        diffusionRate = Mathf.Clamp(diffusionRate, gunData.MinDiffusionRate, gunData.MaxDiffusionRate);
        diffusionRate -= diffusionRate * GameTimer.Instance.GetScaledDeltaTime();

        if (playerInput.GetOnReload() && shotSection != ShotSection.Reload)
        {
            shotSection = ShotSection.Reload;
        }

        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.shot:
                if (playerInput.GetOnClick() && haveGun.BulletNow > 0)
                {
                    GunFire();
                    haveGun.ResetShotTimer();
                    shotSection++;
                }
                else if (haveGun.BulletNow == 0)
                {
                    shotSection = ShotSection.Reload;
                }
                break;
            case ShotSection.shotInterval:
                if (gunData.ShotIntervalTime <= haveGun.ShotTimer)
                {
                    shotSection = ShotSection.shot;
                    haveGun.ResetShotTimer();
                }
                else
                {
                    haveGun.ShotIntervalTimer();
                }
                break;
            case ShotSection.Reload:
                if (gunData.ReloadTypeBolt)
                {
                    BoltReload();
                }
                else
                {
                    MagazineReload();
                }
                break;
            default:
                break;
        }
    }

    private void BoltReload()
    {
        if (playerInput.GetOnClick())
        {
            Debug.Log("ボルト式キャンセル終了 BulletNow :" + haveGun.BulletNow);
            haveGun.ResetReloadTimer();
            shotSection = ShotSection.shot;
        }
        if (gunData.ReloadTime / haveGun.BulletMax <= haveGun.ReloadTimer)
        {
            haveGun.BulletAdd(1);
            if (haveGun.BulletNow == haveGun.BulletMax)
            {
                haveGun.ResetReloadTimer();
                shotSection = ShotSection.shot;
                Debug.Log("ボルト式リロード終了 BulletNows :" + haveGun.BulletNow);
            }
        }
        else
        {
            haveGun.ReloadIntervalTimer();
        }
    }


    private void MagazineReload()
    {
        if (gunData.ReloadTime <= haveGun.ReloadTimer)
        {
            haveGun.BulletAdd(gunData.MaxBullet);
            shotSection = ShotSection.shot;
            Debug.Log("マガジン式リロード終了 BulletNow :" + haveGun.BulletNow);
        }
        else
        {
            haveGun.BulletResume();
            haveGun.ReloadIntervalTimer();
        }
    }

    private void PlayerRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        player.transform.rotation = targetRotation;
    }

    public void ShotRangeView()
    {
        if (mesh != null)
        {
            mesh.Clear();

            // 中心はプレイヤー
            vertices[0] = player.transform.position;

            float angle = diffusionRate * 2f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -diffusionRate + (angle / segment) * i;

                Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
                Vector3 dir = rot * player.transform.up;

                Vector3 point = player.transform.position + dir * viewDistance;

                vertices[i + 1] = point;

                if (i < segment)
                {
                    int start = i * 3;
                    // 中心
                    triangles[start] = 0;
                    // 左上
                    triangles[start + 1] = i + 2;
                    // 右上
                    triangles[start + 2] = i + 1;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }
}