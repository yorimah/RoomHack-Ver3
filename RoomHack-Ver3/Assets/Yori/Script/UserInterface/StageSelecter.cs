using UnityEngine;
using System.Collections.Generic;

public class StageSelecter : MonoBehaviour
{
    [SerializeField, Header("ステージセレクトボタン")]
    private List<WindowStageSelect> selectButtonList = new List<WindowStageSelect>();

    [SerializeField, Header("ステージローダー")]
    private StageDataBank stageDataBank;

    StageData stageData;
    void Start()
    {
        foreach (var selcetButton in selectButtonList)
        {
            int rand = Random.Range(0, stageDataBank.dataList.Count + 1);
            selcetButton.SetScene(stageDataBank.dataList[rand], 3);
            //sceneToLoad.Remove(sceneToLoad[rand]);
        }
    }
}
