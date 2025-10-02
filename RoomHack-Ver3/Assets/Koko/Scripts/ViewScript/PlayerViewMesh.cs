using System.Collections.Generic;
using UnityEngine;

public class PlayerViewMesh : MonoBehaviour
{
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

    private GameObject viewMeshs;


    private void Awake()
    {
        //Init();
        CustumInit();
    }
    private void Init()
    {
        // 空のゲームオブジェクトを生成
        viewMeshs = new GameObject(gameObject.name + "ViewMehs");
        viewMeshs.transform.parent = this.transform;
        for (int i = 0; i < rayValue - 1; i++)
        {
            mesh.Add(new Mesh());
            triangles.Add(Instantiate(triangle, viewMeshs.transform));
            triangles[i].transform.localPosition = Vector3.zero;
            triangles[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }
    }
    private void Update()
    {
        //PlayeViewer();
        PlayerViewerCustum();
    }

    public void GeneradeTriangle(Vector3 _playerOrigin, Vector3 _hitPointFirst, Vector3 _hitpointSecond, int index)
    {
        // 親オブジェクトのローカル空間に変換
        Transform t = triangles[index].transform;
        Vector3[] vertices = new Vector3[]
        {
        t.InverseTransformPoint(_playerOrigin),
        t.InverseTransformPoint(_hitPointFirst),
        t.InverseTransformPoint(_hitpointSecond)
        };

        int[] trianglesIndex = new int[] { 0, 1, 2 };
        mesh[index].Clear();
        mesh[index].vertices = vertices;
        mesh[index].triangles = trianglesIndex;
        mesh[index].RecalculateNormals();
    }

    public void PlayeViewer()
    {
        Vector3 startPos = this.transform.position;
        float partRot = rayRot / (rayValue - 1);
        float nowRot = rayOffset;
        for (int i = 0; i < rayValue; i++)
        {
            Vector3 rayDir = new Vector3(rayLen * Mathf.Cos(nowRot * Mathf.Deg2Rad), rayLen * Mathf.Sin(nowRot * Mathf.Deg2Rad), 0);
            Vector3 rayEnd = startPos + rayDir * rayLen;
            RaycastHit2D result = Physics2D.Linecast(startPos, rayEnd, targetLm);

            if (result.collider != null)
            {
                Items.Add(result.point);
            }
            else
            {
                Items.Add(rayEnd);
            }
            nowRot += partRot;
        }
        for (int i = 1; i < Items.Count; i++)
        {
            GeneradeTriangle(Items[i - 1], startPos, Items[i], i - 1);
        }
        Items.Clear();
    }
    // 分割数
    private int segment = 360;
    private void PlayerViewerCustum()
    {
        custumMesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はプレイヤー
        vertices[0] = UnitCore.Instance.transform.position;

        for (int i = 0; i < segment; i++)
        {
            float angle = (360f / segment) * i;
            float rad = angle * Mathf.Deg2Rad;
            float radius = 15f;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(UnitCore.Instance.transform.position, dir, radius, targetLm);
            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = UnitCore.Instance.transform.position + dir * radius;
            }

            // 三角形の設定
            if (i < segment - 1)
            {
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = i + 2;
                triangles[start + 2] = i + 1;
            }
            else
            {
                // 最後は1番目の点とつなげる
                int start = i * 3;
                triangles[start] = 0;
                triangles[start + 1] = 1;
                triangles[start + 2] = i + 1;
            }
        }
        custumMesh.vertices = vertices;
        custumMesh.triangles = triangles;
        custumMesh.RecalculateNormals();
    }
    private Mesh custumMesh;
    private GameObject meshObject;
    private void CustumInit()
    {
        meshObject = Instantiate(triangle);
        meshObject.transform.localPosition = Vector2.zero;

        custumMesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = custumMesh;
    }
}