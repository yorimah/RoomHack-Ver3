using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
    float timer = 0;

    [SerializeField]
    RectTransform redScreen_Up;

    [SerializeField]
    RectTransform redScreen_Down;

    [SerializeField]
    GameObject[] GameOverText;

    float moveValue;

    private void Start()
    {
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
