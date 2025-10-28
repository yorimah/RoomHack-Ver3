using UnityEngine;
using Zenject;
public class PlayerViewMesh : MonoBehaviour
{
    [SerializeField, Header("視界に使用するメッシュ")]
    GameObject viewMesh;
    [SerializeField, Header("プレイヤー視界の半径")]
    private float viewRadial;
    [SerializeField]
    LayerMask targetLm;
    // 分割数
    private int segment = 360;
    private Mesh mesh;

    [Inject]
    IPosition readPosition;

    ViewerGenarater viewerGenarater;
    private void Awake()
    {
        MeshInit();
    }
    private void MeshInit()
    {
        viewerGenarater = new(viewMesh,this.gameObject,targetLm);
    }
    private void Update()
    {
        // PlayerView();

        //viewerGenarater.CircleViewerUpdate();
        viewerGenarater.CircleViewerUpdate(360);
    }
}