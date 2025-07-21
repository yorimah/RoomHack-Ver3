using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
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

    PlayerSaveData data;

    private void Start()
    {
        // セーブデータ読み込み
        data = SaveManager.Instance.Load();

        GameOverText[2].GetComponent<Text>().text = "stage : " + data.score_Stage;
        GameOverText[3].GetComponent<Text>().text = "destroy : " + data.score_DestoryEnemy;

        // コルーチン起動

        // 遷移完了してからGameOverSequenceを呼ぶ
        GameOverSequence().Forget();
    }
    async UniTask GameOverSequence()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        for (int i = 0; i < 140; i++)
        {
            moveValue++;
            redScreen_Up.anchoredPosition = new Vector2(0, 270 + moveValue * 3);
            redScreen_Down.anchoredPosition = new Vector2(0, -270 - moveValue * 3);
            await UniTask.Yield();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        GameOverText[0].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        GameOverText[1].SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        GameOverText[2].SetActive(true);
        GameOverText[3].SetActive(true);


    }

    private void Update()
    {
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
