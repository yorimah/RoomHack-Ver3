using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSystem : MonoBehaviour
{
    [SerializeField, Header("ToolDataBankをアタッチしてね")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("DeckListをアタッチしてね")]
    ToolDeckList setList;

    public List<tool> toolDeck = new List<tool>();

    public List<tool> toolHand = new List<tool>();

    [SerializeField]
    int handSize = 5;

    public List<tool> toolTrash = new List<tool>();

    private void Start()
    {
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
        toolDeck.AddRange(setList.deckList);
        Shuffle(toolDeck);
    }

    public void Shuffle(List<tool> tools)
    {
        List<tool> processingList = new List<tool>();

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
        Debug.Log("リフレッシュ！");
        toolDeck.AddRange(toolTrash);
        toolTrash.Clear();
        Shuffle(toolDeck);
    }

    public tool DeckDraw()
    {
        tool drawTool;

        if (handSize <= toolHand.Count)
        {
            //Debug.Log("ハンドがあふれちまうおー");
            drawTool = tool.none;
        }
        else
        {
            if (toolDeck.Count <= 0)
            {
                //Debug.Log("デッキ切れ");
                drawTool = tool.none;
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

    public tool HandTrash(int index)
    {
        tool trashTool;

        if (index >= toolHand.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
            trashTool = tool.none;
        }
        else
        {
            trashTool = toolHand[index];
            Debug.Log("トラッシュ！" + trashTool);
            toolTrash.Add(toolHand[index]);
            toolHand.RemoveAt(index);
        }

        return trashTool;
    }

    public tool HandPlay(int index)
    {
        tool playTool;

        if (index >= toolHand.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
            playTool = tool.none;
        }
        else
        {
            //Debug.Log("プレイ！" + toolHand[index]);

            playTool = toolHand[index];
            Instantiate(toolDataBank.toolDataList[(int)toolHand[index]].toolEvent);
        }

        return playTool;
    }

}
