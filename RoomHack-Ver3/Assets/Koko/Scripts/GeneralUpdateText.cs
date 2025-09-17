using UnityEngine;
using UnityEngine.UI;

public class GeneralUpdateText : MonoBehaviour
{
    public string inputText = "hello world";

    public int delay = 0;

    [SerializeField, Tooltip("テキスト開始スイッチ")]
    public bool isRunning = false;

    //アクティブ状態になると動作開始
    [SerializeField, Tooltip("アクティブ時に起動するかどうか")]
    bool activeStart = true;

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
        if (this.gameObject.activeInHierarchy && !isRunning)
        {
            isRunning = true;
            textIndex = 0;
        }

        // 起動中
        if (isRunning)
        {
            // テキスト送り
            if (textIndex < inputText.Length)
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
                textIndex = inputText.Length;
            }

        }
        else
        {
            textIndex = 0;
        }

        // テキスト表示
        dispText.text = inputText.Substring(0, textIndex);
    }
}
