
using UnityEngine;

public class ToolEvent_CCTVHack : ToolEvent
{
    Mesh mesh;
    [SerializeField]
    private float diffusionRate = 120;
    GameObject shotRange;
    // 長さ
    [SerializeField]
    private float viewDistance = 50f;
    // 分割数
    [SerializeField]
    private int segment = 20;

    private void Start()
    {
        shotRange = new GameObject(this.gameObject.name + "shotRange");
        shotRange.AddComponent<MeshRenderer>();
        shotRange.AddComponent<MeshFilter>();
        shotRange.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        shotRange.GetComponent<MeshFilter>().mesh = mesh;

        var mr = shotRange.GetComponent<MeshRenderer>();
        // 仮の色
        mr.material = new Material(Shader.Find("Custom/URP_SpriteSimple"));
        mr.material.color = new Color(1, 1, 0, 0.3f); // 半透明黄色
        mr.sortingOrder = 10;

        //shotRange.transform.parent = this.transform;
    }

    private void Update()
    {
        Tracking();

        // 対象が破壊されたら消したいんやけど初期値で爆発しちまう
        //if (targetObject != null)
        //{
        //    Destroy(this.gameObject);
        //}

        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心はobj
        vertices[0] = this.transform.position;

        float angle = diffusionRate;

        for (int i = 0; i <= segment; i++)
        {
            float diffusionAngle = -diffusionRate + (angle / segment) * i;

            Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
            Vector3 dir = rot * this.transform.up;

            Vector3 point = this.transform.position + dir * viewDistance;

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


    public override void ToolAction()
    {

    }
}
