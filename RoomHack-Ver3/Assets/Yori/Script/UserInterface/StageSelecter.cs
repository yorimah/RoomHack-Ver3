using System.Collections.Generic;
using UnityEngine;

public class StageSelecter : MonoBehaviour
{
    [SerializeField, Header("ステージセレクトボタン")]
    private List<WindowStageSelect> selectButtonList = new List<WindowStageSelect>();

    [SerializeField]
    private StageDataBank stageDataBank;

    [SerializeField, Header("ステージローダー")]
    StageSceneLoader loader;
    void Start()
    {
        foreach (var selcetButton in selectButtonList)
        {
            int rand = Random.Range(0, stageDataBank.dataList.Count);
            selcetButton.SetScene(stageDataBank.dataList[rand], rand, loader);
        }
    }
}
