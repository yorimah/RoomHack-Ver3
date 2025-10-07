using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : MonoBehaviour
{
    [SerializeField, Header("ToolDataBankをアタッチしてね")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("saveから拾ったリスト")]
    List<toolTag> setList = new List<toolTag>();

    public List<toolTag> toolDeck = new List<toolTag>();

    public List<toolTag> toolHand = new List<toolTag>();

    [SerializeField]
    public int handSize = 5;

    public List<toolTag> toolTrash = new List<toolTag>();



    // Singletonパターン
    public static DeckSystem Instance { get; private set; }
    private void Awake()
    {
        // 重複を防止
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }



    private void Start()
    {
        setList = IntToDeck(UnitCore.Instance.data.deckList);

        DeckGenerate();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    DeckDraw();
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    HandPlay(0);
        //    HandTrash(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    HandPlay(1);
        //    HandTrash(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    HandPlay(2);
        //    HandTrash(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    HandPlay(3);
        //    HandTrash(3);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    HandPlay(4);
        //    HandTrash(4);
        //}
    }

    public void DeckGenerate()
    {
        //Debug.Log("デッキ生成！");
        toolDeck.AddRange(setList);
        Shuffle(toolDeck);
    }

    public void Shuffle(List<toolTag> tools)
    {
        List<toolTag> processingList = new List<toolTag>();

        for (int i = 0; i < tools.Count; i++)
        {
            int rand = Random.Range(0, i);

            //Debug.Log("範囲0-" + i+ " 場所" + rand + "に" + tools[i] + "をインサート");

            processingList.Insert(rand, tools[i]);
        }

        tools.Clear();
        tools.AddRange(processingList);
    }

    public void Refresh()
    {
        //Debug.Log("リフレッシュ！");
        toolDeck.AddRange(toolTrash);
        toolTrash.Clear();
        Shuffle(toolDeck);
    }

    public toolTag DeckDraw()
    {
        toolTag drawTool;

        if (handSize <= toolHand.Count)
        {
            //Debug.Log("ハンドがあふれちまうおー");
            drawTool = toolTag.none;
        }
        else
        {
            if (toolDeck.Count <= 0)
            {
                //Debug.Log("デッキ切れ");
                drawTool = toolTag.none;
            }
            else
            {
                drawTool = toolDeck[0];

                //Debug.Log("ドロー！" + drawTool);
                toolHand.Add(toolDeck[0]);
                toolDeck.RemoveAt(0);
            }

        }

        return drawTool;
    }

    public toolTag HandTrash(int index)
    {
        toolTag trashTool;

        if (index >= toolHand.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
            trashTool = toolTag.none;
        }
        else
        {
            trashTool = toolHand[index];
            //Debug.Log("トラッシュ！" + trashTool);
            toolTrash.Add(toolHand[index]);
            toolHand.RemoveAt(index);
        }

        return trashTool;
    }

    public toolTag HandPlay(int index, GameObject _hackObject)
    {
        toolTag playTool;

        if (index >= toolHand.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
            playTool = toolTag.none;
        }
        else
        {
            //Debug.Log("プレイ！" + toolHand[index]);

            playTool = toolHand[index];
            GameObject eventObj = Instantiate(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent, _hackObject.transform.position, Quaternion.identity);
            eventObj.GetComponent<ToolEvent>().targetObject = _hackObject;
        }

        return playTool;
    }

    public int ReturnToolCost(toolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolCost;
    }

    public string ReturnToolText(toolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolText;
    }

    public bool RamUse(int num)
    {
        if (UnitCore.Instance.nowRam >= num)
        {
            UnitCore.Instance.nowRam -= num;
            return true;
        }
        else
        {
            Debug.LogError("コスト足りひんぞ！");
            return false;
        }
    }

    public List<toolTag> IntToDeck(List<int> _list)
    {
        List<toolTag> toolDeckList = new List<toolTag>();

        for (int i = 0; i < _list.Count; i++)
        {
            toolDeckList.Add(toolDataBank.toolDataList[_list[i]].toolTag);
        }

        return toolDeckList;
    }
}
