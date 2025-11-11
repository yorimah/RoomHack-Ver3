
using UnityEngine;

public class ToolEvent_CCTVHack : ToolEventBase, IToolEventBase_Target
{
    // IToolEventBase_Target
    public GameObject hackTargetObject { get; set; }
    public void Tracking(GameObject _gameObject)
    {
        this.transform.position = _gameObject.transform.position;
        this.transform.localEulerAngles = _gameObject.transform.localEulerAngles;
    }


    public override toolTag thisToolTag { get; set; } = toolTag.CCTVHack;

    [SerializeField]
    private float diffusionRate = 60;


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

    ViewerGenarater halfViewerGenarater;
    ViewerGenarater circleViewerGenarater;

    Enemy enemy;
    bool notEnemy;
    private void Start()
    {

    }

    protected override void Enter()
    {
        EventAdd(hackTargetObject);
        halfViewerGenarater = new(meshPrefab, this.gameObject, targetLm, segment, viewDistance);
        circleViewerGenarater = new(meshPrefab, this.gameObject, targetLm, segment, 0.5f);

        if (hackTargetObject.TryGetComponent<Enemy>(out var _enemy))
        {
            enemy = _enemy;
            notEnemy = false;
        }
        else
        {
            notEnemy = true;
        }
    }

    protected override void Execute()
    {
        halfViewerGenarater.CircleViewerUpdate(transform.up, 60);
        circleViewerGenarater.CircleViewerUpdate(transform.up, 360);
        if (!notEnemy)
        {
            if (enemy.died)
            {
                EventEnd();
            }
        }
        Tracking(hackTargetObject);
    }

    protected override void Exit()
    {
        halfViewerGenarater.ViewDestroy();
        circleViewerGenarater.ViewDestroy();
        EventRemove(hackTargetObject);
    }
}
