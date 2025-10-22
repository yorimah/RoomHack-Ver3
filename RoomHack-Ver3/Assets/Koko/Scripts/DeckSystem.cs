using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class DeckSystem : MonoBehaviour
{
    [SerializeField, Header("ToolDataBankをアタッチしてね")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("saveから拾ったリスト")]
    List<toolTag> setList = new List<toolTag>();

    public List<toolTag> toolDeck = new List<toolTag>();

    public List<toolTag> toolHand = new List<toolTag>();

    public List<toolTag> toolTrash = new List<toolTag>();

    //[SerializeField]
    //public int handSize = 5;

    [SerializeField, Header("アタッチしてね")]
    ToolEventManager toolEventManager;

    [Inject]
    IUseableRam useableRam;

    [Inject]
    IDeckList deckListData;

    private void Start()
    {
        setList = IntToDeck(deckListData.DeckList);

        DeckGenerate();
    }

    toolTag ToolMove(List<toolTag> _fromToolList, int _index, List<toolTag> _toToolList)
    {
        toolTag moveTool = toolTag.none;

        if (_fromToolList.Count > _index)
        {
            moveTool = _fromToolList[_index];
            _toToolList.Add(_fromToolList[_index]);
            _fromToolList.RemoveAt(_index);
        }
        else
        {
            Debug.LogError("indexがオーバーフローしてます");
        }

        return moveTool;
    }

    public void Shuffle(List<toolTag> _tools)
    {
        List<toolTag> processingList = new List<toolTag>();

        for (int i = 0; i < _tools.Count; i++)
        {
            int rand = Random.Range(0, i);

            //Debug.Log("範囲0-" + i+ " 場所" + rand + "に" + tools[i] + "をインサート");

            processingList.Insert(rand, _tools[i]);
        }

        _tools.Clear();
        _tools.AddRange(processingList);
    }

    public void DeckGenerate()
    {
        //Debug.Log("デッキ生成！");
        toolDeck.AddRange(setList);
        Shuffle(toolDeck);
    }

    public void TrashRefresh()
    {
        //Debug.Log("リフレッシュ！");
        toolDeck.AddRange(toolTrash);
        toolTrash.Clear();
        Shuffle(toolDeck);
    }

    public toolTag DeckDraw()
    {
        toolTag drawTool;

        //if (handSize <= toolHand.Count)
        //{
        //    //Debug.Log("ハンドがあふれちまうおー");
        //    drawTool = toolTag.none;
        //}
        //else
        {
            if (toolDeck.Count <= 0)
            {
                //Debug.Log("デッキ切れ");
                drawTool = toolTag.none;
            }
            else
            {
                //Debug.Log("ドロー！" + drawTool);
                drawTool = ToolMove(toolDeck, 0, toolHand);
            }
        }

        return drawTool;
    }

    public toolTag HandTrash(int _index)
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

        return ToolMove(toolHand, _index, toolTrash);
    }

    public toolTag HandPlay(int _index, GameObject _target)
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

        toolTag playTool = toolHand[_index];

        //if (playTool != toolTag.none)
        {
            toolEventManager.EventPlay(toolDataBank.toolDataList[(int)toolHand[_index]].toolEvent, _target);
            //HandTrash(_index);
            //ToolMove(toolHand, index, toolTrash);

            //ToolEvent eventObj = Instantiate(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent, _hackObject.transform.position, Quaternion.identity);
            //eventObj.hackTargetObject = _hackObject;
        }

        return playTool;
    }

    public toolType ReturnToolType(toolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolType;
    }

    public int ReturnToolCost(toolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolCost;
    }

    public string ReturnToolText(toolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolText;
    }

    public bool ReturnToolNeedTarget(toolTag _tool)
    {
        if (toolDataBank.toolDataList[(int)_tool].toolEvent.TryGetComponent<IToolEventBase_Target>(out var toolEventBase_Target))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ToolCostUse(toolTag _tool)
    {
        // ダブルでこすとがつかえるかチェック
        float ram = useableRam.NowRam - toolDataBank.toolDataList[(int)_tool].toolCost;
        // 下限を超えないかチェック
        if (ram >= 0)
        {
            useableRam.RamUse(toolDataBank.toolDataList[(int)_tool].toolCost);
        }
        else
        {
            Debug.LogError("RamAddで下限を超えました");
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
