using UnityEngine;
using Zenject;
public class PlayerViewMesh : MonoBehaviour
{
    [SerializeField, Header("視界に使用するメッシュ")]
    GameObject triangle;
    [SerializeField, Header("プレイヤー視界の半径")]
    private float viewRadial;
    [SerializeField]
    LayerMask targetLm;
    // 分割数
    private int segment = 360;
    private Mesh mesh;
    private GameObject meshObject;

    [Inject]
    IPosition readPosition;
    private void Awake()
    {
        MeshInit();
    }
    private void MeshInit()
    {
        meshObject = Instantiate(triangle);
        meshObject.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    private void Update()
    {
        PlayerView();
    }
    private void PlayerView()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はプレイヤー
        vertices[0] = readPosition.PlayerPosition;

        for (int i = 0; i < segment; i++)
        {
            float angle = (360 / segment) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(readPosition.PlayerPosition, dir, viewRadial, targetLm);
            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = readPosition.PlayerPosition + dir * viewRadial;
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
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}