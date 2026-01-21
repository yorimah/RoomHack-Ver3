using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ToolUIController : MonoBehaviour
{

    [SerializeField, Header("アタッチしてね")]
    GameObject toolUIPrefab;


    // nowってついてるやつが今のポジション、それぞれを指定してる
    // HandとDeckのポジションをそれぞれアンカー変更してほしい

    [SerializeField]
    Vector2 nowDeckPos;

    [SerializeField]
    Vector2 nowTrashPos;

    [SerializeField]
    Vector2 nowHandPos;

    [SerializeField]
    Vector2 actDeckPos = new Vector2(-1000, 0);

    [SerializeField]
    Vector2 actTrashPos = new Vector2(-1000, -320);

    [SerializeField]
    Vector2 actHandPos = new Vector2(1000, 0);
    //new Vector2(0, -680)

    [SerializeField]
    Vector2 hackDeckPos = new Vector2(-800, 0);

    [SerializeField]
    Vector2 hackTrashPos = new Vector2(-800, -320);

    [SerializeField]
    Vector2 hackHandPos = new Vector2(800, 0);
    //new Vector2(0, -480)

    [SerializeField]
    float handSpace = 200;

    [SerializeField]
    Vector2 handUIScale = new Vector2(0.8f, 0.8f);

    [SerializeField]
    List<ToolUI> handToolUIList = new List<ToolUI>();

    [SerializeField]
    List<ToolUI> trashToolUIList = new List<ToolUI>();

    List<ToolUI> refreshToolUIList = new List<ToolUI>();

    ToolUI deckSign;
    ToolUI trashSign;

    [SerializeField, Header("デッキ枚数表示テキスト")]
    GeneralUpdateText deckNumText;

    [SerializeField, Header("トラッシュ枚数表示テキスト")]
    GeneralUpdateText trashNumText;

    bool isTrashCheck;

    public List<bool> handCostList = new List<bool>();
    public List<bool> handPlayList = new List<bool>();

    public int handOnIndex = 0;
    public bool isHandOn = false;

    [Inject]
    IUseableRam useableRam;


    public void ToolUIControllerStart()
    {
        nowDeckPos = actDeckPos;
        nowTrashPos = actTrashPos;
        nowHandPos = actHandPos;

        Vector2 signSize = new Vector2(0.5f, 0.5f);

        // デッキ置き場
        GameObject signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = actDeckPos;
        deckSign = signToolUI.GetComponent<ToolUI>();
        deckSign.toMovePosition = actDeckPos;
        deckSign.thisTool = ToolTag.none;
        deckSign.isOpen = false;

        signToolUI.GetComponent<RectTransform>().localScale = signSize;
        deckSign.toScale = signSize;

        // トラッシュ置き場
        signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = actTrashPos;
        trashSign = signToolUI.GetComponent<ToolUI>();
        trashSign.toMovePosition = actTrashPos;
        trashSign.thisTool = ToolTag.none;
        trashSign.isOpen = false;

        signToolUI.GetComponent<RectTransform>().localScale = signSize;
        trashSign.toScale = signSize;

        useableRam.IsRebootSet(true);
    }

    public void ToolUIControllerUpdate()
    {
        // ポジション関連の入力
        deckSign.toMovePosition = nowDeckPos;
        trashSign.toMovePosition = nowTrashPos;

        if (GameTimer.Instance.IsHackTime)
        {
            nowDeckPos = hackDeckPos;
            nowTrashPos = hackTrashPos;

            nowHandPos = hackHandPos;
        }
        else
        {
            nowDeckPos = actDeckPos;
            nowTrashPos = actTrashPos;

            nowHandPos = actHandPos;
        }


        // デッキクリックでリブート開始
        // 廃止
        {
            //if (Input.GetMouseButtonDown(0) && deckSign.isPointerOn)
            //{
            //    SeManager.Instance.Play("RebootStart");
            //    //Debug.Log("Rebooting!");
            //    UnitCore.Instance.isRebooting = true;
            //}
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    SeManager.Instance.Play("RebootStart");
            //    //Debug.Log("Rebooting!");
            //    UnitCore.Instance.isRebooting = true;
            //}
        }

        // リフレッシュリスト処理
        RefreshDestroy();

        HandOnIndexCheck();


        // trashがhandより上じゃないとチェックがバグる
        TrashControl();

        HandControl();
    }

    ToolUI ToolUIGenerate(Vector2 _rectPos, ToolTag _thisTool, bool _isOpen)
    {
        GameObject newToolUIObject = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        newToolUIObject.GetComponent<RectTransform>().localPosition = _rectPos;

        ToolUI newToolUI = newToolUIObject.GetComponent<ToolUI>();
        newToolUI.thisTool = _thisTool;
        newToolUI.isOpen = _isOpen;

        return newToolUI;
    }

    public void TrashRefresh()
    {
        // 山切れ処理
        //refreshToolUIList.AddRange(trashToolUIList);

        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            trashToolUIList[i].toMovePosition = nowDeckPos;
            trashToolUIList[i].isOpen = false;
        }
        //deckSystem.Refresh();
        refreshToolUIList.AddRange(trashToolUIList);
        trashToolUIList.Clear();
    }

    public void DeckDraw(ToolTag _drawTool)
    {
        handToolUIList.Add(ToolUIGenerate(nowDeckPos, _drawTool, false));

        //if (drawTool != toolTag.none)
        //{
        //    handToolUIList.Add(ToolUIGenerate(nowDeckPos, drawTool, false));
        //}
        //else
        //{
        //    //if (deckSystem.toolHand.Count >= playerSaveData.handNum /*deckSystem.handSize*/)
        //    //{
        //    //    Debug.LogError("手札いっぱいだろ！");
        //    //}
        //    //else
        //    {
        //        //Debug.Log("リフレッシュすんぞ！");
        //        Refresh();
        //        //deckSystem.DeckDraw();
        //        //handToolUIList.Add(ToolUIGenerate(deckPos, drawTool, false));
        //    }
        //}
    }

    public void HandTrash(int _index)
    {
        trashToolUIList.Add(handToolUIList[_index]);
        handToolUIList.RemoveAt(_index);

    }

    public void TrashToHand(int _index)
    {
        handToolUIList.Add(trashToolUIList[_index]);
        trashToolUIList.RemoveAt(_index);
    }

    void HandOnIndexCheck()
    {
        isHandOn = false;
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            if (handToolUIList[i].isPointerOn)
            {
                isHandOn = true;
                handOnIndex = i;
            }
        }
    }

    void HandControl()
    {

        // ハンドのToolの位置と見た目
        // ハンドプレイ
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            ToolUI hand = handToolUIList[i];

            // 初期設定
            Vector2 firstHandPos;
            //firstHandPos.x = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            //firstHandPos.y = 0;
            firstHandPos.x = 0;
            firstHandPos.y = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            hand.toScale = handUIScale;
            hand.isTextDisp = false;
            hand.isBlackOut = true;

            //Debug.Log(handCostList[i]);

            // ツールコストが足りるかチェック
            //if (deckSystem.ReturnToolCost(hand.thisTool) > Player.Instance.nowRam)
            //Debug.Log(handCostList[i] + " / " + i);
            if (!handCostList[i])
            {
                //firstHandPos.y = -100;
                firstHandPos.x += 100;
                hand.isBlackOut = true;
            }
            else
            {
                if (handPlayList[i])
                {
                    hand.isBlackOut = false;
                }

                // 対象がカード使えるかチェック
                {
                    //GameObject hackObj = null;
                    //if (cameraPositionController.targetObject != null)
                    //{
                    //    if (cameraPositionController.targetObject.TryGetComponent<IHackObject>(out var ho))
                    //    {
                    //        if (ho.canHackToolTag.Contains(hand.thisTool))
                    //        {
                    //            // 使えるカードは明るくなる
                    //            hand.isBlackOut = false;
                    //            hackObj = cameraPositionController.targetObject;
                    //        }
                    //    }
                    //}
                }

                // 手を重ねると情報出る、位置とサイズ移動
                if (isHandOn && i == handOnIndex)
                {

                    //firstHandPos.y = 100;
                    firstHandPos.x -= 100;
                    hand.toScale = handUIScale * 1.2f;
                    hand.isTextDisp = true;
                    hand.isBlackOut = true;

                }
            }

            hand.toMovePosition = nowHandPos + firstHandPos;

            //hand.thisTool = deckSystem.toolHand[i];
            hand.isOpen = true;
        }
    }

    void TrashControl()
    {
        // トラッシュチェックのオンオフ
        {
            //bool isTrashOn = false;
            //for (int i = 0; i < trashToolUIList.Count; i++)
            //{
            //    if (trashToolUIList[i].isPointerOn) isTrashOn = true;
            //}

            //if (Input.GetKeyDown(KeyCode.Mouse1))
            //{
            //    if (isTrashOn && !isTrashCheck)
            //    {
            //        isTrashCheck = true;
            //    }
            //    else
            //    {
            //        isTrashCheck = false;
            //    }
            //}
        }

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

                //trash.toScale = new Vector2(1f, 1f);
                trash.toScale = new Vector2(0.5f, 0.5f);
                trash.isTextDisp = false;
                trash.isBlackOut = false;

                trash.toMovePosition = nowTrashPos;
            }
        }
    }

    void RefreshDestroy()
    {
        for (int i = 0; i < refreshToolUIList.Count; i++)
        {
            ToolUI refreshTool = refreshToolUIList[i];
            if (refreshTool.isMove == false)
            {
                refreshToolUIList.RemoveAt(i);
                Destroy(refreshTool.gameObject);
            }
        }
    }
}
