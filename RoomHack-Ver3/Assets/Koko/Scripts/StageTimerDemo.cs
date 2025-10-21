using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class StageTimerDemo : MonoBehaviour
{
    [SerializeField]
    float stageTime = 10;

    float timer = 0;

    Text dispText;

    [Inject]
    IGetPlayerDie playerDie;

    private void Start()
    {
        dispText = GetComponent<Text>();
        timer = stageTime;
        //playerDie.PlayerDie += () => { SceneManager.LoadScene("GameOverDemoScene"); };
    }

    private void Update()
    {
        if (timer <= 0)
        {
            timer = 0;
        }

        dispText.text = timer.ToString("00.00");
        timer -= GameTimer.Instance.ScaledDeltaTime;
    }


}
