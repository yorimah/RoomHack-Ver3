using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageSceneLoader : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    StageDataBank stageDataBank;

    PlayerSaveData saveData;

    StageData nowStageData;
    RandomFloorData nowRandomFloorData;

    string loadScene;

    [SerializeField]
    int nowStageNum = 0;
    [SerializeField]
    int nowFloor = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextFloorSceneLoad();
        }
    }

    void NextFloorSceneLoad()
    {
        //// セーブデータをロード
        //saveData = SaveManager.Instance.Load();
        //nowStageNum = saveData.nowStageNum;
        //nowFloor = saveData.nowFloor;

        // 現在のステージを取得
        nowStageData = stageDataBank.dataList[nowStageNum];
        Debug.Log("今のステージは" + nowStageData);

        // 現在のフロアナンバーから次のランダムフロアデータを取得
        for (int i = 0; i < nowStageData.dataList.Count; i++)
        {
            if (nowStageData.dataList[i].floorNo > nowFloor)
            {
                break;
            }
            else
            {
                nowRandomFloorData = nowStageData.dataList[i].randomFloorData;
            }
        }

        Debug.Log("次のランダムフロアデータは" + nowRandomFloorData);

        // 次のフロアをランダムで取得

        // フロア外ならクリアシーンへ
        if (nowStageData.floorNum <= nowFloor)
        {
            loadScene = "ClearDemoScene";
        }

        //Debug.Log("次のシーンは" + loadScene);
        //SceneManager.LoadScene(loadScene);
    }
}
