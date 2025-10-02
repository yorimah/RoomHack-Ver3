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
    List<ToolUI> handToolUIList = new List<ToolUI>();

    [SerializeField]
    List<ToolUI> trashToolUIList = new List<ToolUI>();

    ToolUI deckSign;
    ToolUI trashSign;

    bool trashCheck;

    private void Start()
    {
        GameObject signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = deckPos;

        deckSign = signToolUI.GetComponent<ToolUI>();
        deckSign.toMovePosition = deckPos;
        deckSign.thisTool = tool.none;
        deckSign.isOpen = false;

        signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = trashPos;
        trashSign = signToolUI.GetComponent<ToolUI>();
        trashSign.toMovePosition = trashPos;
        trashSign.thisTool = tool.none;
        trashSign.isOpen = false;
    }

    private void Update()
    {
        if (deckSign.isPointerOn)
        {
            //Debug.Log("されんだー？");
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("kurikku");

                tool drawTool = deckSystem.DeckDraw();

                if(drawTool != tool.none)
                {
                    handToolUIList.Add(ToolUIGenerate(deckPos, drawTool, false));
                }
                else
                {
                    Debug.LogError("おい！引けてねえぞ！");
                }

            }
        }


        // trashのtoolUI位置修正
        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            ToolUI trash = trashToolUIList[i];

            if (trash.isPointerOn || trashSign.isPointerOn) trashCheck = true;
            else trashCheck = false;

            if (trashCheck)
            {
                Debug.Log("トラッシュ内を見れるよー");

                Vector2 firstTrashPos;
                firstTrashPos.x = ((trashToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
                firstTrashPos.y = 480;
                trash.toMovePosition = handPos + firstTrashPos;
            }
            else
            {
                //Debug.Log("トラッシュ通常モード");

                trash.toMovePosition = trashPos;

            }

        }

        // ハンドのToolの位置修正プログラム
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            ToolUI hand = handToolUIList[i];

            Vector2 firstHandPos;
            firstHandPos.x = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            if (hand.isPointerOn == true && !Input.GetMouseButton(0))
            {
                firstHandPos.y = 100;
                hand.toScale = new Vector2(1.2f, 1.2f);
                hand.isTextDisp = true;
            }
            else
            {
                firstHandPos.y = 0;
                hand.toScale = new Vector2(1f, 1f);
                hand.isTextDisp = false;
            }
            hand.toMovePosition = handPos + firstHandPos;

            //hand.thisTool = deckSystem.toolHand[i];
            hand.isOpen = true;
        }

        // ハンドプレイ
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            if (handToolUIList[i].isPointerOn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    tool playTool = deckSystem.HandPlay(i);
                    if (playTool != tool.none)
                    {
                        deckSystem.HandTrash(i);
                        trashToolUIList.Add(handToolUIList[i]);
                        handToolUIList.RemoveAt(i);
                    }
                    else
                    {
                        Debug.LogError("おい！プレイできんかったぞ！");
                    }

                }
            }
        }


    }

    ToolUI ToolUIGenerate(Vector2 _rectPos, tool _thisTool , bool _isOpen)
    {
        GameObject newToolUIObject = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        newToolUIObject.GetComponent<RectTransform>().localPosition = _rectPos;

        ToolUI newToolUI = newToolUIObject.GetComponent<ToolUI>();
        newToolUI.thisTool = _thisTool;
        newToolUI.isOpen = _isOpen;

        return newToolUI;
    }
}
