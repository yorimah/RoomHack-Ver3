using UnityEngine;

public class WindowMove : MonoBehaviour
{
    Vector3 mouseStartPos;
    Vector3 dragStartPos;
    public bool canDrag;

    private RectTransform rectTransform;

    private BoxCollider2D boxCollider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        boxCollider.size = rectTransform.sizeDelta * 0.9f;
    }
    public void DragStart()
    {
        dragStartPos = GetComponent<RectTransform>().position;
    }
    public void DragMove(Vector3 mouseStartPos)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        GetComponent<RectTransform>().position = dragStartPos + mouseVec / 100;
    }

    private void GetMousePositionObject()
    {
        // レイ射出
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);

        foreach (RaycastHit2D hit in hitsss)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.TryGetComponent<WindowMove>(out var moveWindow))
            {
                moveWindow.canDrag = true;
            }
        }
    }
}
