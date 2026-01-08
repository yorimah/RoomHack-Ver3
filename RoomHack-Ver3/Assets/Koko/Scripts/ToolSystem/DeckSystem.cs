using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class DeckSystem : MonoBehaviour
{
    [SerializeField, Header("ToolDataBankをアタッチしてね")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("saveから拾ったリスト")]
    List<ToolTag> setList = new List<ToolTag>();

    public List<ToolTag> toolDeck = new List<ToolTag>();

    public List<ToolTag> toolHand = new List<ToolTag>();

    public List<ToolTag> toolTrash = new List<ToolTag>();

    //[SerializeField]
    //public int handSize = 5;

    [SerializeField, Header("アタッチしてね")]
    ToolEventManager toolEventManager;

    [Inject]
    IUseableRam useableRam;

    [Inject]
    IDeckList deckListData;

    public void DeckSystemStart()
    {
        setList = IntToDeck(deckListData.DeckList);

        DeckGenerate();
    }

    ToolTag ToolMove(List<ToolTag> _fromToolList, int _index, List<ToolTag> _toToolList)
    {
        ToolTag moveTool = ToolTag.none;

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

    public void Shuffle(List<ToolTag> _tools)
    {
        List<ToolTag> processingList = new List<ToolTag>();

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

    public ToolTag DeckDraw()
    {
        ToolTag drawTool;

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
                drawTool = ToolTag.none;
            }
            else
            {
                //Debug.Log("ドロー！" + drawTool);
                drawTool = ToolMove(toolDeck, 0, toolHand);
            }
        }

        return drawTool;
    }

    public ToolTag HandTrash(int _index)
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

    public ToolTag HandPlay(int _index, GameObject _target)
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

        ToolTag playTool = toolHand[_index];

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

    public toolType ReturnToolType(ToolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolType;
    }

    public int ReturnToolCost(ToolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolCost;
    }

    public string ReturnToolText(ToolTag _tool)
    {
        return toolDataBank.toolDataList[(int)_tool].toolText;
    }

    public bool ReturnToolNeedTarget(ToolTag _tool)
    {
        if (toolDataBank.toolDataList[(int)_tool].toolEvent.TryGetComponent<IToolEvent_Target>(out var toolEventBase_Target))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ToolCostUse(ToolTag _tool)
    {
        // ダブルでこすとがつかえるかチェック
        float ram = useableRam.RamNow - toolDataBank.toolDataList[(int)_tool].toolCost;
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

    public List<ToolTag> IntToDeck(List<int> _list)
    {
        List<ToolTag> toolDeckList = new List<ToolTag>();

        for (int i = 0; i < _list.Count; i++)
        {
            toolDeckList.Add(toolDataBank.toolDataList[_list[i]].toolTag);
        }

        return toolDeckList;
    }
}
