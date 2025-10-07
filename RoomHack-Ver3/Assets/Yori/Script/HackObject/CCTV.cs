using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CCTV : MonoBehaviour, IHackObject
{
    // ハッキング実装
    public List<toolTag> canHackToolTag { get; set; } = new List<toolTag>();

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

    public CancellationToken token;
    CancellationTokenSource cts;
    private void Awake()
    {
        // 空のゲームオブジェクトを生成
        viewMeshs = new GameObject(gameObject.name + "ViewMehs");

        viewMeshs.transform.SetParent(this.transform);
        viewMeshs.transform.position = Vector2.zero;
        for (int i = 0; i < rayValue; i++)
        {
            mesh.Add(new Mesh());
            triangles.Add(Instantiate(triangle, Vector2.zero, Quaternion.identity, viewMeshs.transform));
            triangles[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }

        // HackData初期化
        canHackToolTag = new List<toolTag> { toolTag.CCTVHack };

        cts = new CancellationTokenSource();
        token = cts.Token;
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


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;

    }
#endif
}
