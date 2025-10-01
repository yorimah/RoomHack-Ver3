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

    public List<tool> toolDesk = new List<tool>();

    [SerializeField]
    int handSize = 5;

    public List<tool> toolTrash = new List<tool>();

    private void Start()
    {
        DeckGenerate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeckDraw();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandPlay(0);
            HandTrash(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandPlay(1);
            HandTrash(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandPlay(2);
            HandTrash(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HandPlay(3);
            HandTrash(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HandPlay(4);
            HandTrash(4);
        }
    }

    void DeckGenerate()
    {
        //Debug.Log("デッキ生成！");
        toolDeck.AddRange(setList.deckList);
        Shuffle(toolDeck);
    }

    void Shuffle(List<tool> tools)
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

    void DeckDraw()
    {
        if (handSize <= toolDesk.Count)
        {
            //Debug.Log("ハンドがあふれちまうおー");
        }
        else
        {
            if (toolDeck.Count <= 0)
            {
                //Debug.Log("デッキ切れ！リフレッシュ！");
                toolDeck.AddRange(toolTrash);
                toolTrash.Clear();
                Shuffle(toolDeck);
            }

            //Debug.Log("ドロー！" + toolDeck[0]);
            toolDesk.Add(toolDeck[0]);
            toolDeck.RemoveAt(0);
        }
    }

    void HandTrash(int index)
    {
        if (index >= toolDesk.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
        }
        else
        {
            Debug.Log("トラッシュ！" + toolDesk[index]);
            toolTrash.Add(toolDesk[index]);
            toolDesk.RemoveAt(index);
        }
    }

    void HandPlay(int index)
    {
        if (index >= toolDesk.Count)
        {
            //Debug.Log("そんなカードはDeskにゃないぜ！");
        }
        else
        {
            //Debug.Log("プレイ！" + toolDesk[index]);

            Instantiate(toolDataBank.toolDataList[(int)toolDesk[index]].toolEvent);
        }
    }
}
