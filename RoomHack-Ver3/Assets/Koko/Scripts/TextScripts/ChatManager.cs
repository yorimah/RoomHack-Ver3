using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    int chatNumber = 0;

    [SerializeField] List<int> chatSide = new List<int>();
    [SerializeField] List<string> chatText = new List<string>();

    [SerializeField] List<string> tellerName = new List<string>();
    [SerializeField] List<GameObject> tellerIcon = new List<GameObject>();

    [SerializeField] GameObject chatBox_Left;
    [SerializeField] GameObject chatBox_Right;

    [SerializeField] GameObject chatEndObject;

    [SerializeField] GameObject boxPrefab;

    Text text_L;
    Text text_R;

    GameObject nowBox;

    int nowLine = 0;

    string nowText = null;

    int textIndex;

    float timer = 0;
    [SerializeField] float textSpeed = 0.1f;
    bool textSkip = false;
    Vector2 textBoxSize = Vector2.zero;

    private void Start()
    {
        text_L = chatBox_Left.GetComponent<Text>();
        text_R = chatBox_Right.GetComponent<Text>();

        text_L.text = null;
        text_R.text = null;

        // 開始時に一個目のチャットが出るように
        LineBreak(2);
        nowText = chatText[chatNumber];

        nowBox = Instantiate(boxPrefab, gameObject.transform.position, Quaternion.identity, this.transform);
        nowBox.transform.SetAsFirstSibling();

        textBoxSize.y++;
    }

    private void Update()
    {
        // チャット開始
        if (chatNumber < chatText.Count)
        {
            // 待機中
            if (nowText == null)
            {
                // クリック時にテキストスタート
                if (Input.GetMouseButtonDown(0))
                {
                    chatNumber++;

                    // サイドの変更確認
                    if (chatNumber != 0 && chatSide[chatNumber] != chatSide[chatNumber-1])
                    {
                        Debug.Log("交代！");

                        LineBreak(1);

                        textBoxSize = Vector2.zero;

                        nowBox = Instantiate(boxPrefab, gameObject.transform.position, Quaternion.identity, this.transform);
                        nowBox.transform.SetAsFirstSibling();
                    }

                    textBoxSize.y++;
                    LineBreak(1);
                    textIndex = 0;
                    nowText = chatText[chatNumber];
                }
            }
            else
            {
                // テキスト打ち込み

                timer += Time.deltaTime;

                if (timer >= textSpeed || textSkip == true)
                {
                    if (chatSide[chatNumber] == 0)
                    {
                        text_L.text += nowText.Substring(textIndex, 1);
                    }
                    else
                    {
                        text_R.text += nowText.Substring(textIndex, 1);
                    }

                    timer = 0;
                    textIndex++;

                    // テキストボックスの横幅増加
                    // indez最大値は超えない
                    if (textIndex > textBoxSize.x)
                    {
                        textBoxSize.x = textIndex;
                    }
                }

                // クリックでテキストスキップ
                if (Input.GetMouseButtonDown(0))
                {
                    textSkip = true;
                }

                // テキスト打ち込み終了文
                if (textIndex >= nowText.Length)
                {
                    textSkip = false;

                    nowText = null;
                }
            }

            NowBoxRectSet(nowBox, chatNumber, nowLine - (int)textBoxSize.y + 1, textBoxSize);
        }
        else
        {
            // チャット終了
            if (timer >= 1f)
            {
                chatEndObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                timer += Time.deltaTime;
            }

        }
    }

    // 改行
    void LineBreak(int value)
    {
        for (int i = 0; i < value; i++)
        {
            text_L.text += "\n";
            text_R.text += "\n";
            nowLine++;
        }
    }

    // テキストボックスの位置、大きさ
    void NowBoxRectSet(GameObject _nowBox, int _chatNumber, int _line, Vector2 _boxSize)
    {
        Vector2 textSize = new Vector2(50, 55);
        float flameSize = 10;

        RectTransform nowBoxRect = _nowBox.GetComponent<RectTransform>();

        Vector2 rectPos = nowBoxRect.anchoredPosition;
        rectPos.x = (-500 + (textSize.x / 2 * _boxSize.x)) * (chatSide[_chatNumber] == 0 ? 1 : -1);
        rectPos.y = -(textSize.y * _line + textSize.y / 2 * _boxSize.y);
        nowBoxRect.anchoredPosition = rectPos;

        Vector2 rectScale = nowBoxRect.sizeDelta;
        rectScale.x = (textSize.x * _boxSize.x) + flameSize;
        rectScale.y = (textSize.y * _boxSize.y) + flameSize;
        nowBoxRect.sizeDelta = rectScale;
    }

}
