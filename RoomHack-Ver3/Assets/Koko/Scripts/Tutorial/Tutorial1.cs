using UnityEngine;
using System.Collections.Generic;

public class Tutorial1 : MonoBehaviour
{
    [SerializeField, Header("あたっち")]
    TutorialManager tutorialManager;

    [SerializeField]
    TutorialDataList tutorialDataList;

    int index = 1;
    bool isExplain = false;
    bool isIndexStart = false;

    float timer = 0;

    bool isFloorStart = false;

    private void Update()
    {
        // ゲーム起動チェック
        if (GameTimer.Instance.playTime == 1)
        {
            isFloorStart = true;
        }

        // ゲーム起動してから
        if (!isFloorStart) return;

        // 説明オンオフ
        if (isExplain)
        {
            tutorialManager.SetStatus(tutorialDataList.dataList[index]);
            GameTimer.Instance.playTime = 0;
        }
        else
        {
            tutorialManager.SetStatus(tutorialDataList.dataList[0]);
            GameTimer.Instance.playTime = 1;
        }

        switch (index)
        {
            case 0:
                // なにもなし
                index++;
                break;

            case 1:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    timer += Time.deltaTime;
                    Debug.Log(timer);
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 6.5 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

            case 2:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    timer += Time.deltaTime;
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 3 && Input.GetKeyDown(KeyCode.W))
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;
        }
    }
}


