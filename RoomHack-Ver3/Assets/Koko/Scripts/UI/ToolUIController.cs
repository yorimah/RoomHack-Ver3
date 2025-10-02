using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolUIController : MonoBehaviour
{
    [SerializeField]
    DeckSystem deckSystem;

    [SerializeField]
    GameObject toolUIPrefab;

    [SerializeField]
    Vector2 deckPos = new Vector2(-800, 0);

    [SerializeField]
    Vector2 trashPos = new Vector2(-800, -320);

    [SerializeField]
    Vector2 handPos = new Vector2(0, -480);

    [SerializeField]
    float handSpace = 200;

    [SerializeField]
    List<GameObject> toolHandUIList = new List<GameObject>();

    private void Start()
    {
        GameObject newToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        newToolUI.GetComponent<RectTransform>().localPosition = deckPos;
        newToolUI.GetComponent<ToolUI>().toMovePosition = deckPos;
        newToolUI.GetComponent<ToolUI>().thisTool = tool.none;
        newToolUI.GetComponent<ToolUI>().isOpen = false;
    }

    private void Update()
    {
        // カウントが変わったら追加、追加しかできん
        if (deckSystem.toolDesk.Count != toolHandUIList.Count)
        {
            GameObject newToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
            newToolUI.GetComponent<RectTransform>().localPosition = deckPos;
            toolHandUIList.Add(newToolUI);
        }

        // ハンドのToolの位置修正プログラム
        for (int i = 0; i < toolHandUIList.Count; i++)
        {
            ToolUI hand = toolHandUIList[i].GetComponent<ToolUI>();

            Vector2 firstHandPos;
            firstHandPos.x = ((toolHandUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            if (hand.isPointerOn == true)
            {
                firstHandPos.y = 100;
                hand.toScale = new Vector2(1.2f, 1.2f);
            }
            else
            {
                firstHandPos.y = 0;
                hand.toScale = new Vector2(1f, 1f);
            }
            hand.toMovePosition = handPos + firstHandPos;

            hand.isTextDisp = hand.isPointerOn;

            hand.thisTool = deckSystem.toolDesk[i];
            hand.isOpen = true;
        }

    }

}
