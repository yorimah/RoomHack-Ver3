using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowStageSelect : WindowSystem
{

    private string sceneToLoad;

    private int stageRange;

    [SerializeField]
    private GameObject stageTitle;
    private GeneralUpdateText titleText;


    [SerializeField]
    private Text selectButtonText;

    private RandomFloorData randomFloor;

    public void SetScene(StageData setScene, int _stageRange)
    {
        titleText = stageTitle.GetComponent<GeneralUpdateText>();
        titleText.delay = 3;
        randomFloor = setScene.dataList[0].randomFloorData;
        sceneToLoad = randomFloor.dataList[0].floorScene;
        stageRange = setScene.floorNum;
        selectButtonText = selectButtonText.GetComponent<Text>();
        selectButtonText.text = sceneToLoad + " FloorNum : " + stageRange;
    }

    public void Accept()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        saveData.stageRange = stageRange;
        saveData.nowFloor = 0;
        SaveManager.Instance.Save(saveData);
        SceneManager.LoadScene(sceneToLoad);
    }

    public override async UniTask PopUpWindow()
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
        buttonObj.SetActive(true);
        titleText.inputText = sceneToLoad;
        titleText.isRunning = true;
    }

    public override async UniTask FadeOutWindow()
    {
        buttonObj.SetActive(false);
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
        titleText.inputText = " ";
        titleText.isRunning = false;
        buttonObj.SetActive(false);
        windowRect.sizeDelta = Vector2.zero;
        windowRect.localPosition = windowInitPos;
        isClick = false;
    }

}
