using UnityEngine;

public class WindowMove : MonoBehaviour, ICanDrag
{
    Vector3 dragStartPos;
    public bool canDrag;

    private RectTransform rectTransform;

    private BoxCollider2D boxCollider;

    Vector2 sizeDelta;
    Vector3 moveRect;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public void ClickInit()
    {
        sizeDelta = rectTransform.sizeDelta;
        moveRect = rectTransform.localPosition;
        dragStartPos = GetComponent<RectTransform>().position;
    }
    void Update()
    {
        boxCollider.size = rectTransform.sizeDelta * 0.9f;
    }
    public void DragMove(Vector3 mouseStartPos)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        Vector3 nextPos = dragStartPos + mouseVec / 100;

        Vector3 aspect = new Vector3(
            Screen.width - rectTransform.sizeDelta.x,
            Screen.height - rectTransform.sizeDelta.y) / 200;

        rectTransform.position = new Vector3(
            Mathf.Clamp(nextPos.x, -aspect.x, aspect.x),
            Mathf.Clamp(nextPos.y, -aspect.y, aspect.y),
            rectTransform.position.z);
    }

    public void DragScale(Vector2 dragPoint, Vector3 mouseStartPos)
    {
        Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        Vector2 moveVec = new Vector2(
            dragPoint.x * mouseVec.x + sizeDelta.x,
            dragPoint.y * mouseVec.y + sizeDelta.y);
        rectTransform.sizeDelta = moveVec;
        Vector3 move = new Vector3(
           Mathf.Abs(dragPoint.x) * mouseVec.x / 2 + moveRect.x,
           Mathf.Abs(dragPoint.y) * mouseVec.y / 2 + moveRect.y,
           moveRect.z);
        rectTransform.localPosition = move;
    }
}
