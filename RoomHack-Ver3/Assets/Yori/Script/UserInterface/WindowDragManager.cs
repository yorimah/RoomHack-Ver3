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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragScalers = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            dragScalers.Clear();
            GetMousePositionObject();
            mouseStartPos = Input.mousePosition;
            foreach (var dragScaler in dragScalers)
            {
                dragScaler.ClickInit();
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            foreach (var dragScaler in dragScalers)
            {
                Debug.Log(dragScaler);
                dragScaler.DragMove(additionVec, mouseStartPos);
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
            if (hit.collider.TryGetComponent<IDragScaler>(out var dragScaler))
            {
                additionVec += dragScaler.DragVec;
                dragScalers.Add(dragScaler);
            }
        }
    }
}
