using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class DeckDisp : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    ToolUI toolUIPrefab;

    public bool isDisp = false;

    [SerializeField, Header("toolDataBankアタッチ")]
    ToolDataBank toolDataBank;
    public List<ToolTag> inputToolList = new List<ToolTag>();

    List<ToolUI> toolUIList = new List<ToolUI>();

    public Vector2 startPos = Vector2.zero;
    public Vector2 basePos = Vector2.zero;
    public Vector2 spaceOffset = new Vector2(200, 300);
    public int horizonNum = 5;
    [Inject]
    IDeckList deckList;
    public void Start()
    {
        inputToolList = IntToDeck(deckList.DeckList);
    }


    public List<ToolTag> IntToDeck(List<int> _list)
    {
        List<ToolTag> toolDeckList = new List<ToolTag>();

        for (int i = 0; i < _list.Count; i++)
        {
            toolDeckList.Add(toolDataBank.toolDataList[_list[i]].toolTag);
        }

        return toolDeckList;
    }
    private void Update()
    {

        if (isDisp)
        {
            //blind.SetActive(true);

            // プレハブ生成必要枚数以下なら
            while (inputToolList.Count > toolUIList.Count)
            {
                toolUIList.Add(ToolUIGenerate(startPos));
            }

            // ツールの表示設定
            for (int i = 0; i < toolUIList.Count; i++)
            {
                // inputListの数だけ表示
                if (i < inputToolList.Count)
                {
                    // ツール表示変更
                    toolUIList[i].gameObject.SetActive(true);
                    toolUIList[i].thisTool = inputToolList[i];
                    toolUIList[i].isOpen = true;

                    // ツールのポジション計算
                    Vector2 pos = basePos;
                    pos.x =
                        basePos.x
                        - ((float)horizonNum - 1) / 2 * spaceOffset.x
                        + i % horizonNum * spaceOffset.x;
                    pos.y = basePos.y
                        + Mathf.Floor((inputToolList.Count - 1) / horizonNum) / 2 * spaceOffset.y
                        - Mathf.Floor(i / horizonNum) * spaceOffset.y;
                    // 位置決定
                    toolUIList[i].toMovePosition = pos;

                    // もしマウスカーソルが上にあるなら
                    if (toolUIList[i].isPointerOn)
                    {
                        // テキスト詳細表示
                        toolUIList[i].isTextDisp = true;
                        toolUIList[i].isBlackOut = true;
                        toolUIList[i].toScale = new Vector2(1.2f, 1.2f);
                        //Debug.Log(toolUIList[i].thisTool);
                    }
                    else
                    {
                        toolUIList[i].isTextDisp = false;
                        toolUIList[i].isBlackOut = false;
                        toolUIList[i].toScale = new Vector2(1.0f, 1.0f);
                    }
                }
                else
                {
                    // 非表示
                    toolUIList[i].gameObject.SetActive(false);
                    toolUIList[i].toMovePosition = startPos;
                }

            }
        }
        else
        {
            //blind.SetActive(false);

            for (int i = 0; i < toolUIList.Count; i++)
            {
                toolUIList[i].isOpen = false;
                toolUIList[i].toMovePosition = startPos;

                toolUIList[i].isTextDisp = false;
                toolUIList[i].isBlackOut = false;
                toolUIList[i].toScale = new Vector2(1.0f, 1.0f);

                // 目標地点に到達
                if (Vector2.Distance(toolUIList[i].GetComponent<RectTransform>().localPosition, startPos) < 10 /*&& !toolUIList[i].isMove*/)
                {
                    // 表示を消す
                    toolUIList[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void DeckDispButton()
    {
        if (isDisp)
        {
            isDisp = false;
        }
        else
        {
            isDisp = true;
        }
    }
    ToolUI ToolUIGenerate(Vector2 _pos)
    {
        ToolUI instance = Instantiate(toolUIPrefab, _pos, Quaternion.identity, this.transform);
        instance.GetComponent<RectTransform>().localPosition = _pos;
        instance.gameObject.SetActive(false);
        instance.isMovePointable = false;
        return instance;
    }
}
