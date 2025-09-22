using UnityEngine;

public class PlayerShot
{
    private UnitCore unitCore;
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
    public PlayerShot(UnitCore _unitCore)
    {
        unitCore = _unitCore;
        mesh = new Mesh();
        shotRange = new GameObject(unitCore.gameObject.name + "shotRangge");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        shotRange.transform.localPosition = Vector3.zero;
        shotRange.GetComponent<MeshFilter>().mesh = mesh;
    }
    private void GunFire()
    {
        GameObject bulletGameObject = Object.Instantiate(unitCore.bulletPrefab, unitCore.transform.position, Quaternion.identity);

        Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

        BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

        bulletCore.power = unitCore.stoppingPower;
        bulletCore.hitStop = 0.1f;
        bulletCore.HitDamegeLayer = unitCore.HitDamegeLayer;


        float rand = Random.Range(-diffusionRate, diffusionRate);


        Vector2 shotDirection = Quaternion.Euler(0, 0, unitCore.transform.eulerAngles.z + rand) * Vector3.up;
        bulletRigit.velocity = shotDirection * unitCore.bulletSpeed;
        bulletGameObject.transform.up = shotDirection;
    }
    public void Shot()
    {

        ShotRangeView(unitCore.transform.position, Vector2.zero);
        diffusionRate = Mathf.Clamp(diffusionRate, unitCore.minDiffusionRate, unitCore.maxDiffusionRate);

        if (Input.GetKeyDown(KeyCode.R) && shotSection != ShotSection.Reload)
        {
            unitCore.NOWBULLET = 0;
            shotSection = ShotSection.Reload;
        }
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.shot:
                if (Input.GetKey(KeyCode.Mouse0) && unitCore.NOWBULLET > 0)
                {
                    GunFire();
                    shotSection++;
                    unitCore.NOWBULLET--;
                    diffusionRate += unitCore.recoil;
                }
                else if (unitCore.NOWBULLET <= 0)
                {
                    unitCore.NOWBULLET = 0;
                    shotSection = ShotSection.Reload;
                }
                else
                {
                    diffusionRate -= diffusionRate * GameTimer.Instance.ScaledDeltaTime;
                    Debug.Log(diffusionRate);
                }
                break;
            case ShotSection.shotInterval:
                if (unitCore.shotIntervalTime <= timer)
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
                if (unitCore.reloadTime <= timer)
                {
                    unitCore.NOWBULLET = unitCore.MAXBULLET;
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
    public void ShotRangeView(Vector3 _playerOrigin, Vector3 _hitpointSecond)
    {
        float x = 4 * Mathf.Cos(diffusionRate * 2);
        float y = 4 * Mathf.Sin(diffusionRate * 2);

        // 親オブジェクトのローカル空間に変換
        Transform t = unitCore.transform;
        Vector3[] vertices = new Vector3[]
        {
        t.InverseTransformPoint(_playerOrigin),
        t.InverseTransformPoint(new Vector2(x,y)),
        t.InverseTransformPoint(_hitpointSecond)
        };

        int[] trianglesIndex = new int[] { 0, 1, 2 };
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = trianglesIndex;
        mesh.RecalculateNormals();
    }
}