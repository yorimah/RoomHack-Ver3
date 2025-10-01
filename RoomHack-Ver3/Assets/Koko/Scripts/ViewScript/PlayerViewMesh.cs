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
        //CustumInit();
    }

    private void Update()
    {
        PlayeViewer();
        //PlayerViewerCustum();
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
    // 長さ
    private float viewDistance = 3f;
    // 分割数
    private int segment = 360;
    private void PlayerViewerCustum()
    {
        custumMesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はプレイヤー
        vertices[0] = this.transform.position;

        float halfAngle = 360 * 0.5f;

        for (int i = 0; i <= segment; i++)
        {
            float diffusionAngle = -360 + (halfAngle / segment) * i;

            Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
            Vector3 dir = rot * transform.up;

            Vector3 point = transform.position + dir * viewDistance;

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
        custumMesh.vertices = vertices;
        custumMesh.triangles = triangles;
        custumMesh.RecalculateNormals();
    }
    private Mesh custumMesh;
    private GameObject meshObject;
    private void CustumInit()
    {
        meshObject=Instantiate(triangle,transform);
        meshObject.transform.localPosition = Vector2.zero;

        custumMesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = custumMesh;
    }
}