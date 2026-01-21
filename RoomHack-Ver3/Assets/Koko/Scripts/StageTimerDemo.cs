using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class StageTimerDemo : MonoBehaviour
{
    [SerializeField]
    float stageTime = 10;

    float timer = 0;

    public float GetTimer
    {
        get => timer;
        private set { }
    }

    Text dispText;

    private void Start()
    {
        dispText = GetComponent<Text>();
        timer = stageTime;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            timer = 0;
            SceneManager.LoadScene("GameOverDemoScene");
        }

        dispText.text = timer.ToString("00.00");
        timer -= GameTimer.Instance.GetScaledDeltaTime();
    }
}

public class GameTimeHolder
{
    private GameTimeHolder _instance;

    public  GameTimeHolder Instance => _instance ??= new GameTimeHolder();

    private  float gameTime = 10;
    public  float GameTime()
    {
        return gameTime ;
    }

    private async UniTask  GameTimeStart()
    {
        while (true)
        {
            gameTime -= GameTimer.Instance.GetCustomTimeScale();
            await UniTask.Yield();
        }
    }
}