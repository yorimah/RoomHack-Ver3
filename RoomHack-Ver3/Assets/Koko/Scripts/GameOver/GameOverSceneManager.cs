using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSceneManager : MonoBehaviour
{
    [SerializeField]
    RectTransform redScreen_Up;

    [SerializeField]
    RectTransform redScreen_Down;

    [SerializeField]
    GameObject[] GameOverText;

    float moveValue;

    SaveManager saveManager;
    PlayerSaveData data;

    bool sequenceEnd;
    int skipValue = 1;

    private void Start()
    {
        // セーブデータ読み込み
        saveManager = new SaveManager();
        data = saveManager.Load();

        GameOverText[2].GetComponent<Text>().text = "stage : " + data.score_Stage;
        GameOverText[3].GetComponent<Text>().text = "destroy : " + data.score_DestoryEnemy;

        BgmManager.Instance.Play("GameOver");

        // コルーチン起動
        GameOverSequence().Forget();
    }

    async UniTask GameOverSequence()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(skipValue));

        for (int i = 0; i < 100; i++)
        {
            moveValue++;
            redScreen_Up.anchoredPosition = new Vector2(0, 270 + moveValue * 4);
            redScreen_Down.anchoredPosition = new Vector2(0, -270 - moveValue * 4);
            if (skipValue == 1)
            {
                await UniTask.Yield();
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));
        GameOverText[0].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(1f * skipValue));
        GameOverText[1].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));
        GameOverText[2].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));
        GameOverText[3].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(1f * skipValue));
        GameOverText[4].SetActive(true);

        sequenceEnd = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (sequenceEnd == true)
            {
                BgmManager.Instance.StopImmediately();
                saveManager.DeleteSave();
                SceneManager.LoadScene("TitleDemoScene");
            }
            else
            {
                skipValue = 0;
            }
        }



        //timer += Time.deltaTime;

        //if (timer > 1 && timer <= 2)
        //{
        //    float moveValue = (timer - 1) * 400;

        //    redScreen_Up.anchoredPosition = new Vector2(0, 270 + moveValue);
        //    redScreen_Down.anchoredPosition = new Vector2(0, -270 - moveValue);
        //}

        //if (timer > 2.5)
        //{
        //    GameOverText[0].SetActive(true);
        //}

        //if (timer > 3)
        //{
        //    GameOverText[1].SetActive(true);
        //}

        //if (timer > 3.5)
        //{
        //    GameOverText[2].SetActive(true);
        //}
    }


}
