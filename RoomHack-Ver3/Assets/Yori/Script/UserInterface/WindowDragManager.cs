using System.Collections.Generic;
using UnityEngine;
using System;
public class WindowDragManager : MonoBehaviour
{
    [SerializeField]
    Vector2 additionVec;

    [SerializeField]
    Vector3 mouseStartPos;
    private List<ICanDrag> clickDragList = new();
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetMousePositionObject();
            mouseStartPos = Input.mousePosition;
            if (clickDragList?.Count > 0)
            {
                clickDragList[0].ClickInit();
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (clickDragList?.Count > 0)
            {
                if (additionVec.x == 0 && additionVec.y == 0)
                {
                    clickDragList[0].DragMove(mouseStartPos);
                }
                else
                {
                    clickDragList[0].DragScale(additionVec, mouseStartPos);
                }
            }
        }
    }
    private void GetMousePositionObject()
    {
        // レイ射出
        RaycastHit2D[] hits = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        additionVec = Vector2.zero;
        clickDragList.Clear();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<ICanDrag>(out var canDrag))
            {
                clickDragList.Add(canDrag);
            }
            if (hit.collider.TryGetComponent<IDragScaler>(out var dragScaler))
            {
                additionVec += dragScaler.DragVec;
                if (Mathf.Abs(additionVec.x) >= 2 || Mathf.Abs(additionVec.y) >= 2)
                {
                    additionVec -= dragScaler.DragVec;
                }
            }
        }
    }
}
