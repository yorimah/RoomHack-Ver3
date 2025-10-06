using System.Collections.Generic;
using UnityEngine;

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

    List<ToolUI> refreshToolUIList = new List<ToolUI>();

    ToolUI deckSign;
    ToolUI trashSign;

    bool isTrashCheck;

    [SerializeField]
    RamUIDisp ramUIDisp;

    bool isRebootHandStay = false;

    private void Start()
    {
        // デッキ置き場
        GameObject signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = deckPos;
        deckSign = signToolUI.GetComponent<ToolUI>();
        deckSign.toMovePosition = deckPos;
        deckSign.thisTool = tool.none;
        deckSign.isOpen = false;

        // トラッシュ置き場
        signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = trashPos;
        trashSign = signToolUI.GetComponent<ToolUI>();
        trashSign.toMovePosition = trashPos;
        trashSign.thisTool = tool.none;
        trashSign.isOpen = false;
    }

    private void Update()
    {
        // reboot関連
        if (UnitCore.Instance.isRebooting)
        {
            for (int i = 0; i < deckSystem.toolHand.Count; i++)
            {
                deckSystem.HandTrash(0);
                trashToolUIList.Add(handToolUIList[0]);
                handToolUIList.RemoveAt(0);
            }
        }
        else
        {
            // 手札配置
            if (isRebootHandStay)
            {
                if (deckSystem.toolHand.Count < deckSystem.handSize)
                {
                    ToolDraw();
                }
                else
                {
                    isRebootHandStay = false;
                    Debug.Log("rebootEnd!");
                }
            }

            // スペースでリブート開始
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Rebooting!");
                UnitCore.Instance.isRebooting = true;
                isRebootHandStay = true;
            }

        }

        // リフレッシュリスト処理
        for (int i = 0; i < refreshToolUIList.Count; i++)
        {
            ToolUI refreshTool = refreshToolUIList[i];
            if (refreshTool.isMove == false)
            {
                refreshToolUIList.RemoveAt(i);
                Destroy(refreshTool.gameObject);
            }
        }


        // ドロー
        //if (deckSign.isPointerOn)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {

        //        ToolDraw();

        //    }
        //}

        

        // トラッシュチェックのオンオフ
        bool isTrashOn = false;
        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            if (trashToolUIList[i].isPointerOn) isTrashOn = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTrashOn && !isTrashCheck)
            {
                isTrashCheck = true;
            }
            else
            {
                isTrashCheck = false;
            }
        }

        HandControl();

        TrashControl();
    }

    ToolUI ToolUIGenerate(Vector2 _rectPos, tool _thisTool, bool _isOpen)
    {
        GameObject newToolUIObject = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        newToolUIObject.GetComponent<RectTransform>().localPosition = _rectPos;

        ToolUI newToolUI = newToolUIObject.GetComponent<ToolUI>();
        newToolUI.thisTool = _thisTool;
        newToolUI.isOpen = _isOpen;

        return newToolUI;
    }

    void Refresh()
    {
        //refreshToolUIList.AddRange(trashToolUIList);

        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            trashToolUIList[i].toMovePosition = deckPos;
            trashToolUIList[i].isOpen = false;
        }
        deckSystem.Refresh();
        refreshToolUIList.AddRange(trashToolUIList);
        trashToolUIList.Clear();
    }

    void ToolDraw()
    {
        tool drawTool = deckSystem.DeckDraw();

        if (drawTool != tool.none)
        {
            handToolUIList.Add(ToolUIGenerate(deckPos, drawTool, false));
        }
        else
        {
            if (deckSystem.toolHand.Count >= deckSystem.handSize)
            {
                Debug.LogError("手札いっぱいだろ！");
            }
            else
            {
                Debug.Log("リフレッシュすんぞ！");
                Refresh();
                //deckSystem.DeckDraw();
                //handToolUIList.Add(ToolUIGenerate(deckPos, drawTool, false));
            }
        }
    }

    void HandControl()
    {

        bool isHandOn = false;
        // ハンドのToolの位置と見た目
        // ハンドプレイ
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            ToolUI hand = handToolUIList[i];

            Vector2 firstHandPos;
            firstHandPos.x = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            firstHandPos.y = 0;

            // カードが使えるかどうか
            if (deckSystem.ReturnToolCost(hand.thisTool) > UnitCore.Instance.nowRam)
            {
                hand.isBlackOut = true;
            }
            else
            {
                // 手を重ねると情報出る、位置とサイズ移動
                if (hand.isPointerOn == true /* && !Input.GetMouseButton(0) */)
                {
                    firstHandPos.y = 100;
                    hand.toScale = new Vector2(1.2f, 1.2f);
                    hand.isTextDisp = true;
                    hand.isBlackOut = true;

                    ramUIDisp.willUseRam = deckSystem.ReturnToolCost(hand.thisTool);
                    isHandOn = true;

                    // クリックするとカードプレイ
                    if (Input.GetMouseButtonDown(0))
                    {
                        tool playTool = deckSystem.HandPlay(i);
                        if (playTool != tool.none && deckSystem.RamUse(deckSystem.ReturnToolCost(hand.thisTool)))
                        {
                            deckSystem.HandTrash(i);
                            trashToolUIList.Add(hand);
                            handToolUIList.RemoveAt(i);
                        }
                        else
                        {
                            Debug.LogError("おい！プレイできんかったぞ！");
                        }

                    }
                }
                else
                {
                    hand.toScale = new Vector2(1f, 1f);
                    hand.isTextDisp = false;
                    hand.isBlackOut = false;
                }
            }

            hand.toMovePosition = handPos + firstHandPos;

            //hand.thisTool = deckSystem.toolHand[i];
            hand.isOpen = true;
        }

        if (!isHandOn) ramUIDisp.willUseRam = 0;
    }

    void TrashControl()
    {
        // trashのtoolUI位置修正
        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            ToolUI trash = trashToolUIList[i];

            //if (trash.isPointerOn || trashSign.isPointerOn) trashCheck = true;
            //else trashCheck = false;

            if (isTrashCheck)
            {
                //Debug.Log("トラッシュ内を見れるよー");

                Vector2 firstTrashPos;
                firstTrashPos.x = ((trashToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
                firstTrashPos.y = 480;
                trash.toMovePosition = handPos + firstTrashPos;

                if (trash.isPointerOn && !Input.GetMouseButton(0))
                {
                    trash.toScale = new Vector2(1.2f, 1.2f);
                    trash.isTextDisp = true;
                    trash.isBlackOut = true;
                }
                else
                {
                    trash.toScale = new Vector2(1f, 1f);
                    trash.isTextDisp = false;
                    trash.isBlackOut = false;
                }

            }
            else
            {
                //Debug.Log("トラッシュ通常モード");

                trash.toScale = new Vector2(1f, 1f);
                trash.isTextDisp = false;
                trash.isBlackOut = false;

                trash.toMovePosition = trashPos;
            }
        }
    }
}
