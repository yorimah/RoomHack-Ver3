using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class StageSceneLoader : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    StageDataBank stageDataBank;

    StageData nowStageData;
    RandomFloorData nowRandomFloorData;

    string loadScene;

    [SerializeField]
    int nowStageNum = 0;
    [SerializeField]
    int nowFloor = 0;
    [Inject]
    IFloorData floorData;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        NextFloorSceneLoad();
    //    }
    //}

    public string NextFloorSceneLoad()
    {
        // セーブデータをロード detoreruyounisuru
        //saveData = SaveManager.Instance.Load();
        nowStageNum = floorData.SelectStageNo;
        nowFloor = floorData.NowFloor;

        // 現在のステージを取得
        nowStageData = stageDataBank.dataList[nowStageNum];
        //Debug.Log("今のステージは" + nowStageData);

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

        //Debug.Log("次のランダムフロアデータは" + nowRandomFloorData);

        // 次のフロアをランダムで取得
        int totalWaight = 0;
        for (int i = 0; i < nowRandomFloorData.dataList.Count; i++)
        {
            totalWaight += nowRandomFloorData.dataList[i].weight;
        }

        int rand = Random.Range(0, totalWaight);

        int nowWaight = 0;
        for (int i = 0; i < nowRandomFloorData.dataList.Count; i++)
        {
            nowWaight += nowRandomFloorData.dataList[i].weight;
            if (nowWaight > rand)
            {
                loadScene = nowRandomFloorData.dataList[i].floorScene;
                break;
            }
        }

        //Debug.Log("トータルは" + totalWaight + " ランダムは" + rand + " 出た値は" + nowWaight);

        // フロア外ならクリアシーンへ
        //if (nowStageData.floorNum <= nowFloor)
        //{
        //    loadScene = "ClearDemoScene";
        //}

        //Debug.Log("次のシーンは" + loadScene);
        //SceneManager.LoadScene(loadScene);

        return loadScene;
    }
}
