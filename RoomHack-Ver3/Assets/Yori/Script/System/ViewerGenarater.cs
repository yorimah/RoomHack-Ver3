using System.Collections.Generic;
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

    Vector3[] vertices;
    int[] triangles;

    /// <summary>
    /// 生成する視界のプレハブ、生成するオブジェクト、壁として認識するレイヤー、円の分割数、半径。
    /// 表示する視界一つにつき一つ生成すること
    /// </summary>
    /// <param name="viewerPrefab"></param>
    /// <param name="_geneGameObject"></param>
    /// <param name="_targetLayerMask"></param>
    /// <param name="_segment"></param>
    /// <param name="_viewDistance"></param>
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

        vertices = new Vector3[segment + 2];
        triangles = new int[segment * 3];
    }

    public void ViewDestroy()
    {
        mesh.Clear();
        Object.Destroy(viewerObject);
    }

    List<IHackObject> preViewHackObjects = new List<IHackObject>();
    List<IHackObject> nowViewHackObjects = new List<IHackObject>();

    /// <summary>
    /// 初期は円の視界を生成。
    /// </summary>
    /// <param name="viewerAngle"></param>
    public void CircleViewerUpdate(float viewerAngle = 360)
    {
        nowViewHackObjects.Clear();

        if (mesh != null)
        {
            mesh.Clear();

            // 中心は生成するオブジェクト
            vertices[0] = geneGameObject.transform.position;
            float halhAngle = viewerAngle * 0.5f;

            for (int i = 0; i <= segment; i++)
            {
                float diffusionAngle = -halhAngle + (viewerAngle / segment) * i;

                Quaternion rot = Quaternion.AngleAxis(diffusionAngle, Vector3.forward);
                Vector3 dir = rot * geneGameObject.transform.up;

                RaycastHit2D wallHit = Physics2D.Raycast(geneGameObject.transform.position, dir,
                    viewDistance, targetLayerMask);

                if (wallHit.collider != null)
                {
                    // 障害物に当たったらその地点を頂点にする
                    vertices[i + 1] = wallHit.point;
                }
                else
                {
                    // 何もなければ円周上の点
                    vertices[i + 1] = geneGameObject.transform.position + dir * viewDistance;
                }
                RaycastHit2D hackHit = Physics2D.Raycast(geneGameObject.transform.position + dir.normalized / 2, dir,
                    viewDistance);
                if (hackHit.collider != null)
                {
                    // 見つけたリストに入ってなかったら見つけたリストぶち込む
                    if (hackHit.collider.TryGetComponent<IHackObject>(out var hackObject))
                    {
                        if (!nowViewHackObjects.Contains(hackObject))
                        {
                            nowViewHackObjects.Add(hackObject);
                            hackObject.CanHack = true;
                        }
                    }
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

            // 今見つけたやつと前フレームで見つけたやつを見て、なかったらCanHackをfalseにする。
            foreach (var prevHackObj in preViewHackObjects)
            {
                if (!nowViewHackObjects.Contains(prevHackObj))
                {
                    prevHackObj.CanHack = false;
                }
            }
            preViewHackObjects.Clear();
            preViewHackObjects.AddRange(nowViewHackObjects);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }
}
