using UnityEngine;
public class PlayerViewMesh : MonoBehaviour
{
    [SerializeField, Header("視界に使用するメッシュ")]
    GameObject viewMesh;
    [SerializeField, Header("プレイヤー視界の半径")]
    private float viewRadial;
    [SerializeField]
    LayerMask targetLm;
    // 分割数
    private int segment = 720;
    //private Mesh mesh;

    ViewerGenarater viewerGenarater;
    private void Start()
    {
        MeshInit();
    }
    private void MeshInit()
    {
        viewerGenarater = new(viewMesh, this.gameObject, targetLm, segment, viewRadial);
    }
    private void Update()
    {
        viewerGenarater.CircleViewerUpdate(Vector3.one);
    }
}