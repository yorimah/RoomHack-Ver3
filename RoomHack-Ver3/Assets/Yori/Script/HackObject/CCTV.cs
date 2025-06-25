using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour, IHackObject
{
    public int secLevele { get; set; }
    public bool clacked { get; set; }

    public float FireWallCapacity { get; set; }
    public float MaxFireWall { get; set; }
    public float NowFireWall { get; set; }

    public float FireWallRecovaryNum { get; set; }
    List<Mesh> mesh = new List<Mesh>();

    [SerializeField]
    GameObject triangle;

    List<GameObject> triangles = new List<GameObject>();

    [SerializeField]
    float rayRot = 360;
    [SerializeField]
    int rayValue = 36;
    [SerializeField]
    float rayLen = 5;
    [SerializeField]
    float rayOffset = 0;

    [SerializeField]
    LayerMask targetLm;
    List<Vector2> Items = new List<Vector2>();
    [SerializeField, Header("HackData")]
    private HackData hackData;
    private void Awake()
    {
        for (int i = 0; i < rayValue; i++)
        {
            mesh.Add(new Mesh());
            triangles.Add(Instantiate(triangle, Vector2.zero, Quaternion.identity));
            triangles[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }

        // HackData初期化
        MaxFireWall = hackData.MaxFireWall;
        NowFireWall = MaxFireWall;
        FireWallRecovaryNum = hackData.FireWallRecovaryNum;

        clack().Forget();
    }

    // 三角形の頂点設定
    public void GeneradeTriangle(Vector3 _playerOrigin, Vector3 _hitPointFirst, Vector3 _hitpointSecond, int index)
    {
        Vector3[] vertices = new Vector3[] {
            _playerOrigin,
            _hitPointFirst,
            _hitpointSecond
        };
        int[] triangles = new int[] { 0, 1, 2 };

        mesh[index].vertices = vertices;
        mesh[index].triangles = triangles;
        mesh[index].RecalculateNormals();
    }

    // 頂点を計算して、設定する。
    public void CameraViewer()
    {
        Vector3 startPos = gameObject.transform.position;
        float partRot = rayRot / (rayValue - 1);
        float nowRot = rayOffset;
        for (int i = 0; i < rayValue; i++)
        {
            Vector3 rayPos = new Vector3(rayLen * Mathf.Cos(nowRot * Mathf.Deg2Rad), rayLen * Mathf.Sin(nowRot * Mathf.Deg2Rad), 0);
            RaycastHit2D result = Physics2D.Linecast(startPos, startPos + rayPos, targetLm);
            //Debug.DrawLine(startPos, startPos + rayPos, color: Color.red);

            if (result.collider != null)
            {
                Items.Add(result.point);
            }
            nowRot += partRot;
        }
        for (int i = 1; i < Items.Count; i++)
        {
            GeneradeTriangle(Items[i - 1], startPos, Items[i], i - 1);
        }

        Items.Clear();
    }

    public void CapacityOver() => clacked = true;

    public void FireWallRecavary()
    {
        if (NowFireWall >= FireWallCapacity)
        {
            clacked = false;
        }
    }
    async UniTask clack()
    {
        while (true)
        {
            FireWallRecavary();
            await UniTask.WaitUntil(() => clacked);
            CameraViewer();
            await UniTask.Yield();
        }
    }
}
