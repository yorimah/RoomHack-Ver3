using UnityEngine;

public class WindowScaler : MonoBehaviour, IDragScaler
{
    BoxCollider2D dragCollider;
    enum DragPoint
    {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    [SerializeField, Header("つまむ場所")]
    private DragPoint drag;

    public Vector2 DragVec { get; private set; }
    [SerializeField, Header("大きさをを調整するobj")]
    private RectTransform changeRectObj;

    Vector2 sizeDelta;
    Vector3 moveRect;

    public void ClickInit(int hierarchy)
    {
        Hierarchy = hierarchy;
        changeRectObj.transform.SetSiblingIndex(Hierarchy);
        sizeDelta = changeRectObj.sizeDelta;
        moveRect = changeRectObj.localPosition;
    }

    void Start()
    {
        dragCollider = GetComponent<BoxCollider2D>();
        changeRectObj = changeRectObj.GetComponent<RectTransform>();
        switch (drag)
        {
            case DragPoint.LEFT:
                DragVec = new Vector2(-1, 0);
                break;
            case DragPoint.UP:
                DragVec = new Vector2(0, 1);
                break;
            case DragPoint.RIGHT:
                DragVec = new Vector2(1, 0);
                break;
            case DragPoint.DOWN:
                DragVec = new Vector2(0, -1);
                break;
            default:
                break;
        }
    }

    public int Hierarchy { get; private set; }
    void Update()
    {
        if (Hierarchy != changeRectObj.GetSiblingIndex())
        {
            Hierarchy = changeRectObj.GetSiblingIndex();
        }
        switch (drag)
        {
            case DragPoint.LEFT:
            case DragPoint.RIGHT:
                if (dragCollider.size.y != changeRectObj.sizeDelta.y)
                {
                    Vector2 parentObj = new Vector2(dragCollider.size.x, changeRectObj.sizeDelta.y);
                    dragCollider.size = parentObj;
                }
                break;
            case DragPoint.UP:
            case DragPoint.DOWN:

                if (dragCollider.size.x != changeRectObj.sizeDelta.x)
                {
                    Vector2 parentObj = new Vector2(changeRectObj.sizeDelta.x, dragCollider.size.y);
                    dragCollider.size = parentObj;
                }
                break;
            default:
                break;
        }

    }

    [SerializeField,Header("最小サイズ")]
    private Vector2 minSize = new Vector2(150, 100);
    public void DragScale(Vector2 dragPoint, Vector3 mouseStartPos)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;

        float scaleVecX = dragPoint.x * mouseVec.x + sizeDelta.x;
        float scaleVecY = dragPoint.y * mouseVec.y + sizeDelta.y;

        float minScaleX = Mathf.Max(scaleVecX, minSize.x);
        float minScaley = Mathf.Max(scaleVecY, minSize.y);

        Vector2 moveVec = new Vector2(minScaleX, minScaley);
        changeRectObj.sizeDelta = moveVec;

        Vector2 diffSizeDelt = moveVec - sizeDelta;
        Vector3 newPos = moveRect;
        newPos.x += dragPoint.x * diffSizeDelt.x / 2;
        newPos.y += dragPoint.y * diffSizeDelt.y / 2;
        changeRectObj.localPosition = newPos;
    }
}

public interface IDragScaler
{
    public Vector2 DragVec { get; }

    public int Hierarchy { get; }

    public void DragScale(Vector2 dragPoint, Vector3 mouseStartPos);

    public void ClickInit(int hierarchy);

}

public interface ICanDrag
{

    public void ClickInit(int hierarchy);

    public int Hierarchy { get; }

    public void HierarchySet();

    public void DragMove(Vector3 mouseStartPos);
}