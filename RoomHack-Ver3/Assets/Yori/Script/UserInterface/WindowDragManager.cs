using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class WindowDragManager : MonoBehaviour
{
    [SerializeField]
    Vector2 additionVec;

    [SerializeField]
    Vector3 mouseStartPos;
    private List<ICanDrag> dragMoves = new();

    [Inject]
    IGetWindowList getWindowList;

    private List<WindowMove> allDragList = new();

    ICanDrag dragMove;
    IDragScaler dragScale;
    void Start()
    {
        allDragList = getWindowList.WindowMoveList;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetMousePositionObject();
            mouseStartPos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (dragMove != null)
            {
                dragMove.DragMove(mouseStartPos);
            }
            else if (dragScale != null)
            {
                dragScale.DragScale(additionVec, mouseStartPos);
            }
        }
    }
    private List<IDragScaler> dragScalers = new();
    private void GetMousePositionObject()
    {
        // レイ射出、一旦すべて取る
        RaycastHit2D[] hits = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        additionVec = Vector2.zero;
        dragMoves.Clear();
        dragScalers.Clear();
        dragMove = null;
        dragScale = null;

        int dragMoveHierarchy = 0;
        int dragScaleHierarchy = 0;
        foreach (RaycastHit2D hit in hits)
        {
            // ドラッグ、スケールの中でのヒエラルキーが一番高い奴を見る
            if (hit.collider.TryGetComponent<ICanDrag>(out var canDrag))
            {
                dragMoves.Add(canDrag);
                if (dragMoveHierarchy <= canDrag.Hierarchy)
                {
                    dragMoveHierarchy = canDrag.Hierarchy;
                }
            }

            if (hit.collider.TryGetComponent<IDragScaler>(out var dragScaler))
            {
                dragScalers.Add(dragScaler);
                if (dragScaleHierarchy <= dragScaler.Hierarchy)
                {
                    dragScaleHierarchy = dragScaler.Hierarchy;
                }
            }

        }
        // ヒエラルキーの高い方の動かす準備をする
        if (dragMoveHierarchy > dragScaleHierarchy)
        {
            if (dragMoves?.Count > 0)
            {
                foreach (var drag in dragMoves)
                {
                    if (dragMoveHierarchy == drag.Hierarchy)
                    {
                        dragMove = drag;
                    }
                }
                if (dragMove != null)
                {
                    dragMove.ClickInit(allDragList.Count);
                    foreach (var item in allDragList)
                    {
                        item.HierarchySet();
                    }
                }
            }
        }
        else
        {
            if (dragScalers?.Count > 0)
            {
                foreach (var drag in dragScalers)
                {
                    if (drag.Hierarchy == dragScaleHierarchy)
                    {
                        additionVec += drag.DragVec;
                        drag.ClickInit(allDragList.Count);
                        dragScale = drag;
                    }
                }
            }
        }
    }
}

public class WindowListHolder : ISetWindowList, IGetWindowList
{
    public List<WindowMove> WindowMoveList { get; private set; } = new();

    public void AddWindowList(WindowMove window)
    {
        WindowMoveList.Add(window);
    }
    public void RemoveWindowList(WindowMove window)
    {
        WindowMoveList.Remove(window);
    }
}
public interface IGetWindowList
{
    public List<WindowMove> WindowMoveList { get; }
}

public interface ISetWindowList
{
    public void AddWindowList(WindowMove window);
    public void RemoveWindowList(WindowMove window);
}