using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Tutorial2 : MonoBehaviour
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

    [SerializeField]
    CameraPositionController camPosCon;


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

                // "ハッキングについて説明します"
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
                }

                // 説明開始条件
                if (timer > 3.5) isExplain = true;

                // 説明終了条件
                if (timer > 4.5 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

                // "ツールです、マウスホイールで選択を切り替えできます"
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
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Mathf.Abs(Input.mouseScrollDelta.y) > 0.5f)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

                // "RAMです、ツール使用にはRAM要領の制限がかかります"
            case 3:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

                // こちらの監視カメラにVisionHackを使用してください
                // 選択中のツールを右クリックで使用できます
            case 4:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

                // お見事です
                // 監視カメラの視界を通じて壁越しの敵にハッキングができます
            case 5:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                if (true) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

                // ツールの効果は様々です
                // 効率的に活用してください
            case 6:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                // アップデート
                {
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                if (camPosCon.targetObject != true) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;
        }
    }
}