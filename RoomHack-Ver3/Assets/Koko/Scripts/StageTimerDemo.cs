using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageTimerDemo : MonoBehaviour
{
    [SerializeField]
    float stageTime = 10;

    float timer = 0;

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
        timer -= GameTimer.Instance.ScaledDeltaTime;
    }


}
