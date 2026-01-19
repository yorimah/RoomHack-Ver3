using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
public class WindowSystem : MonoBehaviour
{
    protected RectTransform windowRect;

    protected bool isClick = false;

    protected BoxCollider2D dragCollider;

    protected Vector3 windowInitPos;

    [SerializeField]
    private GameObject windowsObject;

    protected float waitSeconds = 0.01f;

    [Inject]
    protected IGetWindowList getWindowList;

    protected WindowMove windowMove;

    public void Start()
    {
        if (windowsObject != null)
        {
            InitWindow(windowsObject);
        }
    }

    protected void InitWindow(GameObject _windowsObject)
    {
        windowsObject = _windowsObject;
        windowMove = windowsObject.GetComponent<WindowMove>();
        dragCollider = windowsObject.GetComponent<BoxCollider2D>();
        dragCollider.enabled = false;
        windowRect = windowsObject.GetComponent<RectTransform>();
        windowInitPos = windowRect.position;
    }
    public void ClickStageSelect()
    {
        if (!isClick)
        {
            //windowRect.transform.SetSiblingIndex(10);
            windowRect.GetComponent<WindowMove>()?.ClickInit(getWindowList.WindowMoveList.Count);
            _ = PopUpWindow();
            //windowRect.sizeDelta = Vector2.zero;
            isClick = true;
        }
    }

    public virtual async UniTask PopUpWindow()
    {
        windowRect.sizeDelta += new Vector2(0, 10);
        while (windowRect.rect.width < Screen.width / 2)
        {
            windowRect.sizeDelta += new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < Screen.height / 2)
        {
            windowRect.sizeDelta += new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        dragCollider.enabled = true;
        windowMove.ButtonSetActive(true);
    }

    public void Minimize()
    {
        if (isClick)
        {
            _ = FadeOutWindow();
            dragCollider.enabled = false;
            isMaximize = false;
        }
    }

    public async void Exit()
    {
        if (isClick)
        {
            await FadeOutWindow();
            windowRect.localPosition = Vector2.zero;
            dragCollider.enabled = false;
            isMaximize = false;
        }
    }

    public virtual async UniTask FadeOutWindow()
    {
        windowMove.ButtonSetActive(false);
        while (windowRect.rect.height > 10)
        {
            windowRect.sizeDelta -= new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.width > 10)
        {
            windowRect.sizeDelta -= new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowMove.ButtonSetActive(false);
        isClick = false;
    }
    Vector2 wasMaximize;
    Vector2 wasMaximizePos;
    public void Maximize()
    {
        if (isClick)
        {
            if (!isMaximize)
            {
                wasMaximize = windowRect.sizeDelta;
                wasMaximizePos = windowRect.localPosition;
                _ = MaximizeWindow();
                isMaximize = true;
            }
            else
            {
                windowRect.sizeDelta = wasMaximize;
                windowRect.localPosition = wasMaximizePos;
                isMaximize = false;
            }
        }
    }
    Vector3 wasPos;
    bool isMaximize = false;
    public void Update()
    {
        //if (isMaximize && windowRect.position != wasPos && isClick)
        //{
        //    windowRect.position = wasPos;
        //    windowRect.sizeDelta = wasMaximize;
        //    isMaximize = false;
        //}


    }
    public async UniTask MaximizeWindow()
    {
        windowRect.localPosition = Vector3.zero;
        //Debug.Log(windowRect.position);
        //Debug.Log(windowRect.transform.position);
        while (windowRect.rect.width < Screen.width)
        {
            windowRect.sizeDelta += new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < Screen.height)
        {
            windowRect.sizeDelta += new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}

