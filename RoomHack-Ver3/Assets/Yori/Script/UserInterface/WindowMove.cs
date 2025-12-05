using UnityEngine;

public class WindowMove : MonoBehaviour
{
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
        Vector3 nextPos = dragStartPos + mouseVec / 100;

        Vector3 aspect = new Vector3(
            Screen.width  - rectTransform.sizeDelta.x ,
            Screen.height  - rectTransform.sizeDelta.y ) / 200;
        rectTransform.position = new Vector3(
            Mathf.Clamp(nextPos.x, -aspect.x, aspect.x),
            Mathf.Clamp(nextPos.y, -aspect.y, aspect.y),
            rectTransform.position.z);
    }
}
