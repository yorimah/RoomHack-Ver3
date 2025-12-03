using UnityEngine;

public class windowScaler : MonoBehaviour
{
    private RectTransform parentObject;
    Vector3 mouseStartPos;
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

    public Vector2 dragVec { get; private set; }
    void Start()
    {
        dragCollider = GetComponent<BoxCollider2D>();
        parentObject = transform.parent.GetComponent<RectTransform>();
        switch (drag)
        {
            case DragPoint.LEFT:
                dragVec = new Vector2(-1, 0);
                break;
            case DragPoint.UP:
                dragVec = new Vector2(0, 1);
                break;
            case DragPoint.RIGHT:
                dragVec = new Vector2(1, 0);
                break;
            case DragPoint.DOWN:
                dragVec = new Vector2(0, -1);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // マウスドラッグ初期設定
            mouseStartPos = Input.mousePosition;
            sizeDelta = parentObject.sizeDelta;
            moveRect = parentObject.localPosition;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            GetMousePositionObject();
        }
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
    private void GetMousePositionObject()
    {
        // レイ射出
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        Vector2 additionVec = Vector2.zero;
        foreach (RaycastHit2D hit in hitsss)
        {
            if (hit.collider.TryGetComponent<windowScaler>(out var moveWindow))
            {
                additionVec += moveWindow.dragVec;
            }
        }
        DragMove(additionVec);
    }

    private void DragMove(Vector2 dragPoint)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        Vector2 moveVec = new Vector2(
            dragPoint.x * mouseVec.x + sizeDelta.x,
            dragPoint.y * mouseVec.y + sizeDelta.y);
        parentObject.sizeDelta = moveVec;
        Vector3 move = new Vector3(
            dragPoint.x * (mouseVec.x / 2) + moveRect.x,
            dragPoint.y * (mouseVec.y / 2) + moveRect.y,
            moveRect.z);
        parentObject.localPosition = move;
    }
}
