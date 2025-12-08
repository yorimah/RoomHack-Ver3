using System.Collections.Generic;
using UnityEngine;

public class WindowScaler : MonoBehaviour, IDragScaler, ICanDrag
{
    public int DragLayer { get; private set; }
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
    [SerializeField, Header("大きさをを調整するobj")]
    private RectTransform changeRectObj;
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
    public void ClickInit()
    {
        sizeDelta = changeRectObj.sizeDelta;
        moveRect = changeRectObj.localPosition;
    }
    void Update()
    {
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


    //public void DragMove(Vector2 dragPoint, Vector3 mouseStartPos)
    //{
    //    Vector3 mouseVec = Input.mousePosition - mouseStartPos;
    //    Vector2 moveVec = new Vector2(
    //        dragPoint.x * mouseVec.x + sizeDelta.x,
    //        dragPoint.y * mouseVec.y + sizeDelta.y);
    //    if (Mathf.Abs(moveRect.x) > 0.5f && Mathf.Abs(moveRect.y) > 0.5f)
    //    {
    //        changeRectObj.sizeDelta = moveVec;
    //        Vector3 move = new Vector3(
    //           Mathf.Abs(dragPoint.x) * mouseVec.x / 2 + moveRect.x,
    //           Mathf.Abs(dragPoint.y) * mouseVec.y / 2 + moveRect.y,
    //           moveRect.z);
    //        changeRectObj.localPosition = move;
    //    }
    //    else
    //    {
    //        Debug.Log("これ以上ちいさくできないよ！" + moveVec);
    //    }
    
    public void DragLayerChange(int changeLayer)
    {
        DragLayer = changeLayer;
    }
}

public interface IDragScaler
{
    public Vector2 DragVec { get; }

    //public void DragMove(Vector2 dragPoint, Vector3 mouseStartPos);

    public void ClickInit();
}

public interface ICanDrag
{
    int DragLayer { get; }

    public void DragLayerChange(int changeLayer);
}

public class DragListHolder : ISetDragList, IGetDragList
{
    List<ICanDrag> dragList = new List<ICanDrag>();


    public void AddDragList(ICanDrag canDrag)
    {
        dragList.Add(canDrag);
        canDrag.DragLayerChange(dragList.IndexOf(canDrag));
    }

    public void ChangeLayerList(ICanDrag canDrag, int changeLayer)
    {
        dragList[canDrag.DragLayer].DragLayerChange(changeLayer);
        dragList.Remove(canDrag);
        dragList.Insert(changeLayer, canDrag);
    }

    public void RemoveDragList(ICanDrag canDrag)
    {
        dragList.Remove(canDrag);
    }

    public List<ICanDrag> GetDragList()
    {
        return dragList;
    }
}

public interface ISetDragList
{
    public void AddDragList(ICanDrag canDrag);

    public void ChangeLayerList(ICanDrag canDrag, int changeLayer);

    public void RemoveDragList(ICanDrag canDrag);
}
public interface IGetDragList
{
    public List<ICanDrag> GetDragList();
}