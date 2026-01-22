using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;
public class StageTimerDemo : MonoBehaviour
{
    [SerializeField]
    float stageTime = 10;

    float timer = 0;

    [Inject]
    ISetTimer setTime;

    [Inject]
    IGetTime getTime;

    public float GetTimer
    {
        get => timer;
        private set { }
    }

    Text dispText;

    private void Start()
    {
        dispText = GetComponent<Text>();
    }

    private void Update()
    {
        setTime.GameTime();

        dispText.text = getTime.gameTime.ToString("00.00");
    }
}

public class GameTimeHolder:ISetTimer,IGetTime
{

    public float gameTime { get; private set; }

    public GameTimeHolder()
    {
        gameTime = 10;
    }

    public void GameTime() 
    {
        gameTime -= GameTimer.Instance.GetScaledDeltaTime();
    }
}

public interface ISetTimer
{
    public  void GameTime();
}

public interface IGetTime
{
    public float gameTime { get; }
}