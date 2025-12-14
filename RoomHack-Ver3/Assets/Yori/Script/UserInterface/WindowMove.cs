using UnityEngine;
using Zenject;
public class WindowMove : MonoBehaviour, ICanDrag
{
    Vector3 dragStartPos;
    public bool canDrag;

    private RectTransform rectTransform;

    private BoxCollider2D boxCollider;

    public int Hierarchy { get; private set; }

    [Inject]
    IAddWindoList addWindoList;
    public void Awake()
    {
        addWindoList.AddWindowList(this);
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        Hierarchy = transform.GetSiblingIndex();
    }
    public void ClickInit(int hierarchy)
    {
        Hierarchy = hierarchy;
        transform.SetSiblingIndex(Hierarchy);
        dragStartPos = GetComponent<RectTransform>().position;
    }

    public void HierarchySet()
    {
        Hierarchy = transform.GetSiblingIndex();
    }
    void Update()
    {
        boxCollider.size = rectTransform.sizeDelta * 0.9f;
        if (Hierarchy != transform.GetSiblingIndex())
        {
            Hierarchy = transform.GetSiblingIndex();
        }
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
}
