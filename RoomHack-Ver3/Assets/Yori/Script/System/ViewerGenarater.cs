using UnityEngine;

public class ViewerGenarater
{
    // 長さ
    private float viewDistance;
    // 分割数
    private int segment;

    Mesh mesh;

    GameObject geneGameObject;

    LayerMask targetLayerMask;

    GameObject viewerObject;
    /// <summary>
    /// 生成する視界のプレハブ、生成するオブジェクト、壁として認識するレイヤー、円の分割数、半径。
    /// 表示する視界一つにつき一つ生成すること
    /// </summary>
    /// <param name="viewerPrefab"></param>
    /// <param name="_geneGameObject"></param>
    /// <param name="_targetLayerMask"></param>
    /// <param name="segment"></param>
    /// <param name="viewDistance"></param>
    public ViewerGenarater(GameObject viewerPrefab, GameObject _geneGameObject, LayerMask _targetLayerMask,
        int _segment = 360, float _viewDistance = 20f)
    {
        geneGameObject = _geneGameObject;

        targetLayerMask = _targetLayerMask;

        segment = _segment;

        viewDistance = _viewDistance;

        viewerObject = Object.Instantiate(viewerPrefab);
        viewerObject.gameObject.name = geneGameObject.name + ": Viewer";
        viewerObject.transform.localPosition = Vector2.zero;

        mesh = new Mesh();
        viewerObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void ViewDestroy()
    {
        mesh.Clear();
        Object.Destroy(viewerObject);
    }

    /// <summary>
    /// 初期は円の視界を生成。
    /// </summary>
    /// <param name="viewerAngle"></param>
    public void CircleViewerUpdate(float viewerAngle = 360)
    {
        if (mesh != null)
        {
            mesh.Clear();

            Vector3[] vertices = new Vector3[segment + 2];
            int[] triangles = new int[segment * 3];

            // 中心は生成するオブジェクト
            vertices[0] = geneGameObject.transform.position;
            float halhAngle = viewerAngle * 0.5f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -halhAngle + (viewerAngle / segment) * i;

                Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
                Vector3 dir = rot * geneGameObject.transform.up;

                RaycastHit2D hit = Physics2D.Raycast(geneGameObject.transform.position, dir,
                    viewDistance, targetLayerMask);
                if (hit.collider != null)
                {
                    // 障害物に当たったらその地点を頂点にする
                    vertices[i + 1] = hit.point;
                }
                else
                {
                    // 何もなければ円周上の点
                    vertices[i + 1] = geneGameObject.transform.position + dir * viewDistance;
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
    }
}
