using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
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

    public float viewAngle = 90f;
    public float viewDistance = 3f;
    public int segment = 20;
    public PlayerShot(UnitCore _unitCore)
    {
        unitCore = _unitCore;
        //mesh = new Mesh();
        shotRange = new GameObject(unitCore.gameObject.name + "shotRangge");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        //shotRange.transform.parent = unitCore.transform;
        shotRange.transform.localPosition =Vector2.zero;
        //shotRange.GetComponent<MeshFilter>().mesh = mesh;

        mesh = new Mesh();
        shotRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = shotRange.GetComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Unlit/Color"));
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色
        mr.sortingOrder = 10;
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

        ShotRangeView();
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
    public void ShotRangeView()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        vertices[0] = unitCore.transform.position;

        float halfAngle = diffusionRate * 2f;

        for (int i = 0; i <= segment; i++)
        {
            float angle = -diffusionRate + (halfAngle / segment) * i;

            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 dir = rot * unitCore.transform.up;

            Vector3 point = unitCore.transform.position + dir * viewDistance;
            //float rad = angle * Mathf.Deg2Rad;

            //Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            vertices[i + 1] = point;

            if (i < segment)
            {
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = i + 2;
                triangles[start + 2] = i + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}