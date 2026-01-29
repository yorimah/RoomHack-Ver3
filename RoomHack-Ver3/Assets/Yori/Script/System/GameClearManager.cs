using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    [SerializeField]
    RectTransform redScreen_Up;

    [SerializeField]
    RectTransform redScreen_Down;

    [SerializeField]
    GameObject[] GameOverText;

    float moveValue;

    PlayerSaveData data;

    bool sequenceEnd;
    int skipValue = 1;

    private CancellationTokenSource cancellationTokenSource;
    float plusTrace = 5;
    float trace;
    private void Start()
    {
        // セーブデータ読み込み
        data = SaveManager.Instance.Load();
        data.trace += plusTrace;
        GameOverText[2].GetComponent<Text>().text = "stage : " + data.score_Stage;
        GameOverText[3].GetComponent<Text>().text = "destroy : " + data.score_DestoryEnemy;

        //BgmManager.Instance.Play("GameOver");
        cancellationTokenSource = new CancellationTokenSource();
        // コルーチン起動

        var linked = CancellationTokenSource.CreateLinkedTokenSource(
        cancellationTokenSource.Token,
        this.GetCancellationTokenOnDestroy()
         );

        _ = GameOverSequence(linked.Token);
    }
    async UniTask GameOverSequence(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(skipValue), cancellationToken: token);

            for (int i = 0; i < 100; i++)
            {
                moveValue++;

                token.ThrowIfCancellationRequested();

                redScreen_Up.anchoredPosition = new Vector2(0, 270 + moveValue * 4);
                redScreen_Down.anchoredPosition = new Vector2(0, -270 - moveValue * 4);
                if (skipValue == 1)
                {
                    await UniTask.Yield(token);
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
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));
            GameOverText[4].SetActive(true);
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusTrace " + plusTrace.ToString("F1");
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));

            while (trace < data.trace)
            {
                trace += 0.1f;
                plusTrace -= 0.1f;
                if (plusTrace <= 0)
                {
                    plusTrace = 0;
                }
                if (trace >= data.trace)
                {
                    trace = data.trace;
                }
                GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusTrace " + plusTrace.ToString("F1");
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f * skipValue));
            }
            trace = data.trace;
            plusTrace = 0;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusTrace " + plusTrace.ToString("F1");
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusTrace " + plusTrace.ToString("F0");
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusTrac";
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + PlusT";
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1") + " + Pl";
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f * skipValue));
            GameOverText[4].GetComponent<Text>().text = "Trace : " + trace.ToString("F1");
            await UniTask.Delay(TimeSpan.FromSeconds(1f * skipValue));
            GameOverText[5].SetActive(true);

            sequenceEnd = true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("終了！");
            throw;
        }

    }

    [SerializeField]
    StageDataBank stageDataBank;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (sequenceEnd == true)
            {
                cancellationTokenSource.Cancel();
                data.maney += stageDataBank.dataList[data.selectStageNo].reward;
                SaveManager.Instance.Save(data);
                BgmManager.Instance.StopImmediately();
                SceneManager.LoadScene("YoriTestScene");
            }
            else
            {
                skipValue = 0;
            }
        }
    }
}
