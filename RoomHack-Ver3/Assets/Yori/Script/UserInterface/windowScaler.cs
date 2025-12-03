using UnityEngine;

public class windowScaler : MonoBehaviour, IDragScaler
{
    private RectTransform parentObject;
    public bool canDrag;

    Vector2 sizeDelta;
    Vector3 moveRect;

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
    void Start()
    {
        dragCollider = GetComponent<BoxCollider2D>();
        parentObject = transform.parent.GetComponent<RectTransform>();
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
    public void ClickInit()
    {
        sizeDelta = parentObject.sizeDelta;
        moveRect = parentObject.localPosition;
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    // マウスドラッグ初期設定
        //    //mouseStartPos = Input.mousePosition;

        //}

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    GetMousePositionObject();
        //}
        switch (drag)
        {
            case DragPoint.LEFT:
            case DragPoint.RIGHT:
                if (dragCollider.size.y != parentObject.sizeDelta.y)
                {
                    Vector2 parentObj = new Vector2(dragCollider.size.x, parentObject.sizeDelta.y);
                    dragCollider.size = parentObj;
                }
                break;
            case DragPoint.UP:
            case DragPoint.DOWN:

                if (dragCollider.size.x != parentObject.sizeDelta.x)
                {
                    Vector2 parentObj = new Vector2(parentObject.sizeDelta.x, dragCollider.size.y);
                    dragCollider.size = parentObj;
                }
                break;
            default:
                break;
        }

    }


    public void DragMove(Vector2 dragPoint, Vector3 mouseStartPos)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        Vector2 moveVec = new Vector2(
            dragPoint.x * mouseVec.x + sizeDelta.x,
            dragPoint.y * mouseVec.y + sizeDelta.y);
        parentObject.sizeDelta = moveVec;
        Vector3 move = new Vector3(
            dragPoint.x * mouseVec.x / 2 + moveRect.x,
            dragPoint.y * mouseVec.y / 2 + moveRect.y,
            moveRect.z);
        parentObject.localPosition = move;
    }
}

public interface IDragScaler
{
    public Vector2 DragVec { get; }

    public void DragMove(Vector2 dragPoint, Vector3 mouseStartPos);

    public void ClickInit();
}
