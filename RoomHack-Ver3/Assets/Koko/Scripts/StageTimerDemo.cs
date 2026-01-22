using UnityEngine;
using UnityEngine.UI;
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

    [Inject]
    ISetHitPoint setHitPoint;
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

        if (getTime.gameTime <= 0)
        {
            setHitPoint.DeadLineDamage();
        }
    }


}
public class GameTimeHolder : ISetTimer, IGetTime
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
    public void GameTime();
}

public interface IGetTime
{
    public float gameTime { get; }
}