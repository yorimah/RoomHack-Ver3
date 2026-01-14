using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class WindowStageSelect : WindowSystem
{
    private string sceneToLoad;

    private int stageRange;

    [SerializeField, Header("テキスト")]
    private GeneralUpdateText titleUpdate;
    [SerializeField]
    private GeneralUpdateText explainUpdate;

    private Text titleText;
    private Text explainText;
    [TextArea(3, 4)]
    private string explainString;
    private string titleString;

    StageSceneLoader loader;

    private int setStageNo;
    [SerializeField, Header("windowSize")]
    private Vector2 setWindowSize;
    public void SetScene(StageData setScene, int _setStageNo, StageSceneLoader _loader)
    {
        setStageNo = _setStageNo;
        loader = _loader;
        titleUpdate.delay = 3;
        explainUpdate.delay = 3;
        sceneToLoad = setScene.stageName;
        stageRange = setScene.floorNum;
        titleText = titleUpdate.GetComponent<Text>();
        explainText = titleUpdate.GetComponent<Text>();

        titleString = sceneToLoad + " FloorNum : " + stageRange;
        titleText.text = titleString;
        explainString = setScene.stageExplain + "\n" + "Reward : " + setScene.reward + "  Level : " + setScene.stageLevel;
        explainText.text = explainString;
    }

    public void Accept()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        saveData.stageRange = stageRange;
        saveData.selectStageNo = setStageNo;
        saveData.nowFloor = 0;
        SaveManager.Instance.Save(saveData);
        SceneManager.LoadScene(loader.NextFloorSceneLoad());
    }

    public override async UniTask PopUpWindow()
    {
        windowRect.sizeDelta += new Vector2(0, 10);
        while (windowRect.rect.width < setWindowSize.x)
        {
            windowRect.sizeDelta += new Vector2(setWindowSize.x / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.height < setWindowSize.y)
        {
            windowRect.sizeDelta += new Vector2(0, setWindowSize.y / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowRect.sizeDelta = setWindowSize;
        dragCollider.enabled = true;
        buttonObj.SetActive(true);
        titleUpdate.inputText = titleString;
        explainUpdate.inputText = explainString;
        titleUpdate.isRunning = true;
        explainUpdate.isRunning = true;
    }

    public override async UniTask FadeOutWindow()
    {
        buttonObj.SetActive(false);
        while (windowRect.rect.height > 100)
        {
            windowRect.sizeDelta -= new Vector2(0, setWindowSize.y / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.width > 100)
        {
            windowRect.sizeDelta -= new Vector2(setWindowSize.x / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        titleUpdate.inputText = " ";
        explainUpdate.inputText = " ";
        titleUpdate.isRunning = false;
        explainUpdate.isRunning = false;
        buttonObj.SetActive(false);
        windowRect.sizeDelta = Vector2.zero;
        windowRect.localPosition = windowInitPos;
        isClick = false;
    }

}
