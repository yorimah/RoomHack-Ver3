using UnityEngine;
using Zenject;
public class WindowMove : MonoBehaviour, ICanDrag
{
    Vector3 dragStartPos;
    public bool canDrag;

    private RectTransform rectTransform;

    //private BoxCollider2D boxCollider;

    public int Hierarchy { get; private set; }

    [Inject]
    ISetWindowList setWindoList;

    [SerializeField]
    GameObject button;

    public RectTransform target;

    Vector2 startPos;

    protected float waitSeconds = 0.01f;
    public void Awake()
    {
        if (setWindoList != null)
        {
            setWindoList.AddWindowList(this);
        }
    }

    public void ButtonSetActive(bool setActive)
    {
        button.SetActive(setActive);
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //boxCollider = GetComponent<BoxCollider2D>();
        Hierarchy = transform.GetSiblingIndex();
    }
    public void ClickInit(int hierarchy)
    {
        //Hierarchy = hierarchy;
        transform.SetSiblingIndex(Hierarchy);
        //dragStartPos = GetComponent<RectTransform>().position;
        startPos = target.anchoredPosition;
    }

    public void HierarchySet()
    {
        Hierarchy = transform.GetSiblingIndex();
    }
    void Update()
    {
        //Vector2 size = boxCollider.size;
        //size.x = rectTransform.sizeDelta.x;
        //if (boxCollider.size!=size )
        //{
        //    boxCollider.size = size;
        //}

        //// boxCollider.size = rectTransform.sizeDelta * 0.9f;
        //if (Hierarchy != transform.GetSiblingIndex())
        //{
        //    Hierarchy = transform.GetSiblingIndex();
        //}
        //if (rectTransform.sizeDelta.x <= 0 && boxCollider.enabled)
        //{
        //    boxCollider.enabled = false;
        //    button.SetActive(false);
        //}
        //if (rectTransform.sizeDelta.x >= 0 && !boxCollider.enabled)
        //{
        //    boxCollider.enabled = true;
        //}
    }
    public void DragMove(Vector3 mouseStartPos)
    {
        //Vector3 mouseVec = Input.mousePosition - mouseStartPos;
        //Vector3 nextPos = dragStartPos + mouseVec / 100;

        //Vector3 aspect = new Vector3(
        //    Screen.width - rectTransform.sizeDelta.x,
        //    Screen.height - rectTransform.sizeDelta.y) / 200;

        //rectTransform.position = new Vector3(
        //    Mathf.Clamp(nextPos.x, -aspect.x, aspect.x),
        //    Mathf.Clamp(nextPos.y, -aspect.y, aspect.y),
        //    rectTransform.position.z);

        Vector2 delta = Input.mousePosition - mouseStartPos;
        target.anchoredPosition = startPos + delta;
    }
    public void Exit()
    {
        setWindoList.RemoveWindowList(this);
        Destroy(gameObject);
    }
}
