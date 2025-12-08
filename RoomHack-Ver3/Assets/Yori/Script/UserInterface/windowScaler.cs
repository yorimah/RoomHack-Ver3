using System.Collections.Generic;
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
}

public interface IDragScaler
{
    public Vector2 DragVec { get; }

    //public void DragMove(Vector2 dragPoint, Vector3 mouseStartPos);

}

public interface ICanDrag
{

    public void ClickInit();


    public void DragMove(Vector3 mouseStartPos);

    public void DragScale(Vector2 dragPoint, Vector3 mouseStartPos);
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