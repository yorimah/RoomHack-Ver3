using UnityEngine;
using UnityEngine.UI;

public class GeneralUpdateText : MonoBehaviour
{
    public string inputText = "hello world";
    public int delay = 0;
    public bool isRunning = false;

    string nowText;

    int timer = 0;
    int textIndex = 0;
    Text dispText;
    
    void Start()
    {
        dispText = GetComponent<Text>();
        if (dispText == null)
        {
            Debug.LogError("テキストにアタッチしてください : " + this.gameObject);
        }
    }
    
    void Update()
    {
        // テキストが変更されたらリセット
        if (nowText != inputText)
        {
            nowText = inputText;
            dispText.text = null;
            textIndex = 0;
        }

        // 起動中
        if (isRunning && nowText != null)
        {
            // テキスト送り
            if (textIndex < nowText.Length)
            {
                if (timer >= delay)
                {
                    textIndex++;
                    timer = 0;
                }

                timer++;
            }

            // ディレイ0なら即座に全文出る
            if (delay == 0)
            {
                textIndex = nowText.Length;
            }

        }
        else
        {
            textIndex = 0;
        }

        // テキスト表示
        dispText.text = nowText.Substring(0, textIndex);
    }
}
