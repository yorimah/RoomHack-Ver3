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

    private float timer = 0;
    private float diffusionRate;

    private GameObject shotRange;

    // 長さ
    private float viewDistance = 3f;
    // 分割数
    private int segment = 20;

    GunData gunData;

    private int nowBullet;
    private int maxBullet;

    private int hitDamageLayer = 1;
    private Material shotRanageMaterial;
    private GameObject bulletPre;

    IPlayerInput playerInput;
    public PlayerShot(GunData _gunData, Material _shotRanageMaterial, GameObject _bulletPre,
        GameObject _player, IPlayerInput _playerInput)
    {
        player = _player;
        playerInput = _playerInput;
        gunData = _gunData;
        maxBullet = gunData.MaxBullet;
        nowBullet = maxBullet;
        shotRanageMaterial = _shotRanageMaterial;
        shotRange = new GameObject(player.gameObject.name + "shotRangge");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        shotRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        shotRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = shotRange.GetComponent<MeshRenderer>();
        // 仮の色
        mr.material = new Material(shotRanageMaterial);
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色
        mr.sortingOrder = 10;

        bulletPre = _bulletPre;
    }
    private void GunFire()
    {
        GameObject bulletGameObject = Object.Instantiate(bulletPre, player.transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.power = gunData.Power;
        bulletCore.hitStop = 0.1f;
        bulletCore.hitDamegeLayer = hitDamageLayer;


        float rand = Random.Range(-diffusionRate, diffusionRate);


        Vector2 shotDirection = Quaternion.Euler(0, 0, player.transform.eulerAngles.z + rand) * Vector3.up;
        bulletRigit.linearVelocity = shotDirection * gunData.BulletSpeed;
        bulletGameObject.transform.up = shotDirection;
    }
    public void Shot()
    {
        ShotRangeView();
        diffusionRate = Mathf.Clamp(diffusionRate, gunData.MinDiffusionRate, gunData.MaxDiffusionRate);
        //diffusionRate -= diffusionRate * GameTimer.Instance.ScaledDeltaTime;

        if (playerInput.GetOnReload() && shotSection != ShotSection.Reload)
        {
            nowBullet = 0;
            shotSection = ShotSection.Reload;
        }
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.shot:
                if (playerInput.GetOnClick() && nowBullet > 0)
                {
                    GunFire();
                    shotSection++;
                    nowBullet--;
                    diffusionRate += gunData.Recoil;
                }
                else if (nowBullet <= 0)
                {
                    nowBullet = 0;
                    shotSection = ShotSection.Reload;
                }
                break;
            case ShotSection.shotInterval:
                if (gunData.ShotIntervalTime <= timer)
                {
                    shotSection = ShotSection.shot;
                    timer = 0;
                }
                else
                {
                    timer += GameTimer.Instance.ScaledDeltaTime;
                }
                break;
            case ShotSection.Reload:
                if (gunData.ReloadTime <= timer)
                {
                    nowBullet = maxBullet;
                    timer = 0;
                    shotSection = ShotSection.shot;
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

    public void ShotRangeView()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

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