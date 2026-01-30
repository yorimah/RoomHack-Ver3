using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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


    [Inject]
    IPosition playerPos;

    [Inject]
    IGetEnemyList enemyList;
    List<EnemyBase> enemies = new List<EnemyBase>();

    [Inject]
    IUseableRam ram;

    

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

        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("Tutorial0-3_saveedit");

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
                    ram.RamUse(ram.RamNow);
                    Debug.Log("しゃおれーい！");

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
                if (timer > 1 && Input.GetKeyDown(KeyCode.W))
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

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
                if (playerPos.PlayerPosition.x > 0 && playerPos.PlayerPosition.y < 2.5) isExplain = true;

                // 説明終了条件
                if (timer > 1 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;

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
                    timer += Time.deltaTime;
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
                    timer += Time.deltaTime;
                }

                // 説明開始条件
                if (timer > 3) isExplain = true;

                // 説明終了条件
                if (timer > 6 && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;
                }

                break;


            case 6:
                // スタート処理
                if (!isIndexStart)
                {
                    // なにかかく
                    timer = 0;

                    enemies.AddRange(enemyList.GetEnemies());

                    isIndexStart = true;
                }

                // アップデート
                {
                    if (isExplain) timer += Time.deltaTime;
                }

                // 説明開始条件
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].isDead) isExplain = true;
                }

                // 説明終了条件
                if (timer > 1 && Input.anyKeyDown)
                {
                    isExplain = false;
                    isIndexStart = false;
                    index++;

                    //PlayerSaveData playerSave = SaveManager.Instance.Load();
                    //playerSave.gunName = GunName.AssuleRifle;
                    //SaveManager.Instance.Save(playerSave);

                }

                break;

            case 7:

                // クリア演出
                if (!isIndexStart)
                {
                    Debug.Log("おわりのはじまり");
                    // なにかかく
                    timer = 0;

                    isIndexStart = true;
                }

                bool isClear = true;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (!enemies[i].isDead) isClear = false;
                }

                Debug.Log(isClear);
                if (isClear) timer += Time.deltaTime;

                if (timer > 1.8f) SceneManager.LoadScene("Tutorial0-2_saveedit");

                break;
        }
    }
}


