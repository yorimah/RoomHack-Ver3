using UnityEngine;

public class BombCore : MonoBehaviour
{
    [SerializeField, Header("爆発半径")]
    public float explosionRadial = 3;
    [SerializeField, Header("爆発威力")]
    public int explosionPower;
    private int segment = 360;
    private Mesh mesh;
    protected GameObject meshObject;
    [SerializeField, Header("表示マテリアル")]
    private Material expRadialMaterial;

    [SerializeField]
    LayerMask targetLm;

    [SerializeField, Header("ExprosionPrefab")]
    private GameObject blastGameObject;

    private BombBlast bombBlast;
    public void Bomb()
    {
        SeManager.Instance.Play("Explosion");
        bombBlast = Instantiate(blastGameObject, this.transform.position, Quaternion.identity).GetComponent<BombBlast>();
        bombBlast.explosionPower = explosionPower;
        bombBlast.explosionRadial = explosionRadial;
        bombBlast.isExplosion = true;
    }

    protected void MeshInit()
    {
        meshObject = new GameObject(gameObject.name + " ExplosionRadiusMesh");
        meshObject.AddComponent<MeshFilter>();
        var mr = meshObject.AddComponent<MeshRenderer>();
        // メッシュの色設定、ここでいじれる
        // フォルダ階層で捜してアタッチするとビルド時に階層が変わってエラーはく
        mr.material = new Material(expRadialMaterial);
        mr.material.color = new Color(0.75f, 0, 0, 0.5f);
        mr.sortingOrder = -1;
        mesh = new Mesh();
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    protected void ExplosionRadius()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segment + 2];
        int[] triangles = new int[segment * 3];

        // 中心は自分
        vertices[0] = transform.position;

        for (int i = 0; i < segment; i++)
        {
            float angle = (360f / segment) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, explosionRadial, targetLm);
            if (hit.collider != null)
            {
                // 障害物に当たったらその地点を頂点にする
                vertices[i + 1] = hit.point;
            }
            else
            {
                // 何もなければ円周上の点
                vertices[i + 1] = transform.position + dir * explosionRadial;
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
