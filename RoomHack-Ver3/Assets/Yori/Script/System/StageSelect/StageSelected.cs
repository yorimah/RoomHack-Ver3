using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageSelected : MonoBehaviour
{
    private RectTransform windowRect;

    [SerializeField]
    private RectTransform backGlound;

    [SerializeField]
    private GameObject buttomObj;

    private RectTransform buttomRect;

    private bool isClick = false;

    private BoxCollider2D dragCollider;

    Vector3 windowInitPos;

    [SerializeField]
    private GameObject windowsObject;

    float waitSeconds = 0.01f;
    private string sceneToLoad;

    private int stageRange;
    public void SetScene(string setScene, int _stageRange)
    {
        sceneToLoad = setScene;
        stageRange = _stageRange;
    }


    public void Start()
    {
        dragCollider = windowsObject.GetComponent<BoxCollider2D>();
        dragCollider.enabled = false;
        windowRect = windowsObject.GetComponent<RectTransform>();
        windowInitPos = windowRect.transform.position;
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
        while (windowRect.rect.height > 100)
        {
            windowRect.sizeDelta -= new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.width > 100)
        {
            windowRect.sizeDelta -= new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        buttomObj.SetActive(false);
        windowRect.sizeDelta = Vector2.zero;
        windowRect.transform.position = windowInitPos;
        isClick = false;
    }
    Vector2 wasMaximize;
    public void Maximize()
    {
        if (isClick)
        {
            if (!isMaximize)
            {
                wasPos = windowRect.transform.position;
                wasMaximize = windowRect.sizeDelta;
                _ = MaximizeWindow();
                isMaximize = true;
            }
            else
            {
                windowRect.sizeDelta = wasMaximize;
                isMaximize = false;
            }
        }
    }
    Vector3 wasPos;
    bool isMaximize = false;
    public void Update()
    {
        if (isMaximize && windowRect.transform.position != wasPos && isClick)
        {
            windowRect.transform.position = wasPos;
            windowRect.sizeDelta = wasMaximize;
            isMaximize = false;
        }

        // windowobjのサイズが変わったらサイズを合わせる
        if (backGlound.sizeDelta != windowRect.sizeDelta)
        {
            backGlound.sizeDelta = windowRect.sizeDelta;
        }

        if (buttomRect.sizeDelta != windowRect.sizeDelta)
        {
            buttomRect.sizeDelta = windowRect.sizeDelta;
            dragCollider.size = new Vector2(windowRect.sizeDelta.x, 40);
            dragCollider.offset = new Vector2(0, windowRect.sizeDelta.y / 2f - dragCollider.size.y / 2f);
        }

    }
    public async UniTask MaximizeWindow()
    {
        windowRect.transform.position = windowInitPos;
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

    public void Accept()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        saveData.stageRange = stageRange;
        saveData.nowFloor = 0;
        SaveManager.Instance.Save(saveData);
        SceneManager.LoadScene(sceneToLoad);
    }
}

