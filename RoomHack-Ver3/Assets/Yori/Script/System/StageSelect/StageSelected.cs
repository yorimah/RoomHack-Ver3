using Cysharp.Threading.Tasks;
using UnityEngine;
public class StageSelected : MonoBehaviour
{
    /// <summary>
    /// あとで変更べつスクリプトで変更できるようにする
    /// </summary>
    [SerializeField, Header("ポップアップするオブジェクト")]
    private RectTransform windowObject;

    [SerializeField]
    private RectTransform backGlound;

    [SerializeField]
    private GameObject buttomObj;
    private RectTransform buttomRect;

    private bool isClick = false;

    private BoxCollider2D dragCollider;

    Vector3 windowInitPos;

    float waitSeconds = 0.01f;
    public void Start()
    {
        dragCollider = windowObject.GetComponent<BoxCollider2D>();
        dragCollider.enabled = false;
        windowInitPos = windowObject.transform.position;
        buttomRect = buttomObj.GetComponent<RectTransform>();
    }
    public void ClickStageSelect()
    {
        if (!isClick)
        {
            _ = PopUpStageSelect();
            isClick = true;
        }
    }

    public async UniTask PopUpStageSelect()
    {
        windowObject.sizeDelta += new Vector2(0, 10);
        while (windowObject.rect.width < Screen.width / 2)
        {
            windowObject.sizeDelta += new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowObject.rect.height < Screen.height / 2)
        {
            windowObject.sizeDelta += new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowObject.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        dragCollider.enabled = true;
        buttomObj.SetActive(true);
    }

    public void Exit()
    {
        if (isClick)
        {
            _ = FadeOutWindow();
            dragCollider.enabled = false;
        }
    }

    public async UniTask FadeOutWindow()
    {
        buttomObj.SetActive(false);
        while (windowObject.rect.height > 100)
        {
            windowObject.sizeDelta -= new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowObject.rect.width > 100)
        {
            windowObject.sizeDelta -= new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        buttomObj.SetActive(false);
        windowObject.sizeDelta = Vector2.zero;
        windowObject.transform.position = windowInitPos;
        isClick = false;
    }
    Vector2 wasMaximize;
    public void Maximize()
    {
        if (isClick)
        {
            if (!isMaximize)
            {
                wasPos = windowObject.transform.position;
                wasMaximize = windowObject.sizeDelta;
                _ = MaximizeWindow();
                isMaximize = true;
            }
            else
            {
                windowObject.sizeDelta = wasMaximize;
                isMaximize = false;
            }
        }
    }
    Vector3 wasPos;
    bool isMaximize = false;
    public void Update()
    {
        if (isMaximize && windowObject.transform.position != wasPos && isClick)
        {
            windowObject.transform.position = wasPos;
            windowObject.sizeDelta = wasMaximize;
            isMaximize = false;
        }


        // windowobjのサイズが変わったらサイズを合わせる
        if (backGlound.sizeDelta != windowObject.sizeDelta)
        {
            backGlound.sizeDelta = windowObject.sizeDelta;
        }

        if (buttomRect.sizeDelta != windowObject.sizeDelta)
        {
            buttomRect.sizeDelta = windowObject.sizeDelta;
            dragCollider.size = new Vector2(windowObject.sizeDelta.x, 40);
            dragCollider.offset = new Vector2(0, windowObject.sizeDelta.y / 2f - dragCollider.size.y / 2f);
        }

    }
    public async UniTask MaximizeWindow()
    {
        windowObject.transform.position = windowInitPos;
        while (windowObject.rect.width < Screen.width)
        {
            windowObject.sizeDelta += new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowObject.rect.height < Screen.height)
        {
            windowObject.sizeDelta += new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowObject.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}

