
using UnityEngine;

public class ToolEvent_CCTVHack : ToolEvent
{
    Mesh mesh;
    [SerializeField]
    private float diffusionRate = 60;

    GameObject viewRange;

    // 長さ
    [SerializeField]
    private float viewDistance = 50f;
    // 分割数
    [SerializeField]
    private int segment = 60;
    [SerializeField, Header("視界に使うメッシュ")]
    private GameObject meshPrefab;

    [SerializeField, Header("壁レイヤー")]
    private LayerMask targetLm;

    private void Start()
    {
        //viewRange = new GameObject(this.gameObject.name + "View");
        viewRange = Instantiate(meshPrefab);
        //viewRange.AddComponent<MeshRenderer>();
        //viewRange.AddComponent<MeshFilter>();
        viewRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        viewRange.GetComponent<MeshFilter>().mesh = mesh;

        //var mr = viewRange.GetComponent<MeshRenderer>();

        //mr.material = new Material(Shader.Find("Custom/WriteStencil"));

        //viewRange.layer = playerViewLayer;

        //shotRange.transform.parent = this.transform;

        miniView = Instantiate(meshPrefab);
        miniView.transform.localPosition = Vector2.zero;
        miniMesh = new Mesh();
        miniView.GetComponent<MeshFilter>().mesh = miniMesh;

        EventAdd();
    }

    private void Update()
    {
        Tracking();
        PlayerView();

        // 対象が破壊されたら消す
        if (hackTargetObject.activeSelf == false)
        {
            EventRemove();
        }

        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はobj
        vertices[0] = this.transform.position;

        float angle = diffusionRate * 2;

        for (int i = 0; i <= segment; i++)
        {
            float diffusionAngle = -diffusionRate + (angle / segment) * i;

            Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
            Vector3 dir = rot * this.transform.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, targetLm);
            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = UnitCore.Instance.transform.position + dir * viewDistance;
            }


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

    GameObject miniView;
    float viewRadial = 0.5f;
    Mesh miniMesh;
    private void PlayerView()
    {
        miniMesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はプレイヤー
        vertices[0] = this.transform.position;

        for (int i = 0; i < segment; i++)
        {
            float angle = (360f / segment) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, viewRadial, targetLm);
            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = this.transform.position + dir * viewRadial;
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
        miniMesh.vertices = vertices;
        miniMesh.triangles = triangles;
        miniMesh.RecalculateNormals();
    }


    public override void ToolAction()
    {

    }
}
