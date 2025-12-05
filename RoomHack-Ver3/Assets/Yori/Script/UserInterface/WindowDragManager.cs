using System.Collections.Generic;
using UnityEngine;

public class WindowDragManager : MonoBehaviour
{
    [SerializeField]
    Vector2 additionVec;

    [SerializeField]
    private List<IDragScaler> dragScalers;

    [SerializeField]
    Vector3 mouseStartPos;

    WindowMove windowMove;
    void Start()
    {
        dragScalers = new();
    }
    CursorMode CursorMode;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            dragScalers.Clear();
            windowMove = null;
            GetMousePositionObject();
            mouseStartPos = Input.mousePosition;
            foreach (var dragScaler in dragScalers)
            {
                dragScaler.ClickInit();
            }
            if (windowMove != null)
            {
                windowMove.DragStart();
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (windowMove != null)
            {
                windowMove.DragMove(mouseStartPos);
            }
            else if (dragScalers?.Count > 0)
            {
                dragScalers[0].DragMove(additionVec, mouseStartPos);
            }
        }
    }


    private void GetMousePositionObject()
    {
        // レイ射出
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        additionVec = Vector2.zero;
        foreach (RaycastHit2D hit in hitsss)
        {
            if (hit.collider.TryGetComponent<WindowMove>(out var _windowMove))
            {
                windowMove = _windowMove;
            }
            else if (hit.collider.TryGetComponent<IDragScaler>(out var dragScaler))
            {
                additionVec += dragScaler.DragVec;
                if (Mathf.Abs(additionVec.x) >= 2 || Mathf.Abs(additionVec.y) >= 2)
                {
                    additionVec -= dragScaler.DragVec;
                }
                else
                {
                    dragScalers.Add(dragScaler);
                }
            }
        }
    }
}
