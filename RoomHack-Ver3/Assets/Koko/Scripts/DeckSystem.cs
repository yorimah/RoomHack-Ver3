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

    public List<toolTag> toolTrash = new List<toolTag>();

    [SerializeField]
    public int handSize = 5;

    [SerializeField]
    ToolEventManager toolEventManager;

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
    public toolTag MoveTool(List<toolTag> fromToolList, int index, List<toolTag> toToolList)
    {
        toolTag moveTool = toolTag.none;

        if (fromToolList.Count > index)
        {
            moveTool = fromToolList[index];
            toToolList.Add(fromToolList[index]);
            fromToolList.RemoveAt(index);
        }
        else
        {
            Debug.LogError("indexがオーバーフローしてます");
        }

        return moveTool;
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


    public void DeckGenerate()
    {
        //Debug.Log("デッキ生成！");
        toolDeck.AddRange(setList);
        Shuffle(toolDeck);
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
                //Debug.Log("ドロー！" + drawTool);
                drawTool = MoveTool(toolDeck, 0, toolHand);
            }
        }

        return drawTool;
    }

    public toolTag HandTrash(int index)
    {
        //toolTag trashTool;

        //if (index >= toolHand.Count)
        //{
        //    //Debug.Log("そんなカードはDeskにゃないぜ！");
        //    trashTool = toolTag.none;
        //}
        //else
        //{
        //    trashTool = toolHand[index];
        //    //Debug.Log("トラッシュ！" + trashTool);
        //    toolTrash.Add(toolHand[index]);
        //    toolHand.RemoveAt(index);
        //}

        return MoveTool(toolHand, index, toolTrash);
    }

    public toolTag HandPlay(int index, GameObject _target)
    {
        //toolTag playTool;

        //if (index >= toolHand.Count)
        //{
        //    //Debug.Log("そんなカードはDeskにゃないぜ！");
        //    playTool = toolTag.none;
        //}
        //else
        //{
        //    //Debug.Log("プレイ！" + toolHand[index]);

        //    playTool = toolHand[index];
        //    GameObject eventObj = Instantiate(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent, _hackObject.transform.position, Quaternion.identity);
        //    eventObj.GetComponent<ToolEvent>().hackTargetObject = _hackObject;
        //}

        //return playTool;

        toolTag playTool = toolHand[index];

        if (playTool != toolTag.none)
        {
            toolEventManager.PlayEvent(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent, _target);
            MoveTool(toolHand, index, toolTrash);

            //ToolEvent eventObj = Instantiate(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent, _hackObject.transform.position, Quaternion.identity);
            //eventObj.hackTargetObject = _hackObject;
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

    public bool RamUse(int useRam, int nowRam)
    {
        if (nowRam >= useRam)
        {
            nowRam -= useRam;
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
