using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
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

    [Inject]
    IStatusSave statusSave;
    public void SetScene(StageData setScene, int _setStageNo, StageSceneLoader _loader)
    {
        Debug.Log("a");
        setStageNo = _setStageNo;
        loader = _loader;
        titleUpdate.delay = 3;
        explainUpdate.delay = 3;
        sceneToLoad = setScene.stageName;
        Debug.Log($"[SetScene] sceneToLoad = {sceneToLoad}");
        stageRange = setScene.floorNum;
        titleText = titleUpdate.GetComponent<Text>();
        explainText = explainUpdate.GetComponent<Text>();

        titleString = sceneToLoad + " FloorNum : " + stageRange;
        titleText.text = titleString;
        explainString = setScene.stageExplain + "\n" + "Reward : " + setScene.reward + "  Level : " + setScene.stageLevel;
        explainText.text = explainString;
    }
    public void Accept()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        saveData.selectStageNo = setStageNo;
        saveData.nowFloor = 0;
        statusSave.HitPointInit();
        SaveManager.Instance.Save(statusSave.playerSave());
        SceneManager.LoadScene(loader.NextFloorSceneLoad());
    }

    protected override void OnOpened()
    {
        titleUpdate.inputText = titleString;
        explainUpdate.inputText = explainString;
        titleUpdate.isRunning = true;
        explainUpdate.isRunning = true;
    }

    protected override void OnClosed()
    {
        titleUpdate.inputText = " ";
        explainUpdate.inputText = " ";
        titleUpdate.isRunning = false;
        explainUpdate.isRunning = false;
    }
}
