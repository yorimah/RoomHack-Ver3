using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ToolManager : MonoBehaviour
{
    DeckSystem deckSystem;

    [SerializeField, Header("アタッチしてね")]
    GameObject toolUIPrefab;

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
    Vector2 actHandPos = new Vector2(0, -680);

    [SerializeField]
    Vector2 hackDeckPos = new Vector2(-800, 0);

    [SerializeField]
    Vector2 hackTrashPos = new Vector2(-800, -320);

    [SerializeField]
    Vector2 hackHandPos = new Vector2(0, -480);



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

    [SerializeField, Header("アタッチしてね")]
    RamUIDisp ramUIDisp;

    bool isRebootHandStay = false;

    [SerializeField, Header("アタッチしてね")]
    CameraPositionController cameraPositionController;

    [Inject]
    IUseableRam useableRam;
    private void Start()
    {
        if (DeckSystem.Instance != null)
        {
            deckSystem = DeckSystem.Instance;
        }
        else
        {
            Debug.LogError("デッキシステムないお");
        }

        // デッキ置き場
        GameObject signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = actDeckPos;
        deckSign = signToolUI.GetComponent<ToolUI>();
        deckSign.toMovePosition = actDeckPos;
        deckSign.thisTool = toolTag.none;
        deckSign.isOpen = false;

        // トラッシュ置き場
        signToolUI = Instantiate(toolUIPrefab, Vector3.zero, Quaternion.identity, this.transform);
        signToolUI.GetComponent<RectTransform>().localPosition = actTrashPos;
        trashSign = signToolUI.GetComponent<ToolUI>();
        trashSign.toMovePosition = actTrashPos;
        trashSign.thisTool = toolTag.none;
        trashSign.isOpen = false;

        useableRam.setIsRebooting(true);
    }

    private void Update()
    {
        deckSign.toMovePosition = nowDeckPos;
        trashSign.toMovePosition = nowTrashPos;

        if (GameTimer.Instance.customTimeScale <= 0.5f)
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

        // reboot関連
        if (useableRam.IsRebooting)
        {
            for (int i = 0; i < deckSystem.toolHand.Count; i++)
            {
                deckSystem.HandTrash(0);
                trashToolUIList.Add(handToolUIList[0]);
                handToolUIList.RemoveAt(0);
            }
            isRebootHandStay = true;
        }
        else
        {
            // 手札再配置
            if (isRebootHandStay)
            {
                SeManager.Instance.Play("RebootEnd");
                if (deckSystem.toolHand.Count < deckSystem.handSize)
                {
                    ToolDraw();
                }
                else
                {
                    isRebootHandStay = false;
                    //Debug.Log("rebootEnd!");
                }
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


        // trashが上じゃないとチェックがバグる
        TrashControl();

        HandControl();
    }

    ToolUI ToolUIGenerate(Vector2 _rectPos, toolTag _thisTool, bool _isOpen)
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
        // 山切れ処理
        //refreshToolUIList.AddRange(trashToolUIList);

        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            trashToolUIList[i].toMovePosition = nowDeckPos;
            trashToolUIList[i].isOpen = false;
        }
        deckSystem.Refresh();
        refreshToolUIList.AddRange(trashToolUIList);
        trashToolUIList.Clear();
    }

    void ToolDraw()
    {
        toolTag drawTool = deckSystem.DeckDraw();

        if (drawTool != toolTag.none)
        {
            handToolUIList.Add(ToolUIGenerate(nowDeckPos, drawTool, false));
        }
        else
        {
            if (deckSystem.toolHand.Count >= deckSystem.handSize)
            {
                Debug.LogError("手札いっぱいだろ！");
            }
            else
            {
                //Debug.Log("リフレッシュすんぞ！");
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

            // 初期設定
            Vector2 firstHandPos;
            firstHandPos.x = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            firstHandPos.y = 0;
            hand.toScale = new Vector2(1f, 1f);
            hand.isTextDisp = false;
            hand.isBlackOut = true;

            // ツールコストが足りるかチェック
            if (deckSystem.ReturnToolCost(hand.thisTool) > useableRam.NowRam)
            {
                hand.isBlackOut = true;
                firstHandPos.y = -100;
            }
            else
            {
                // 対象がカード使えるかチェック
                GameObject hackObj = null;
                if (cameraPositionController.targetObject != null)
                {
                    if (cameraPositionController.targetObject.TryGetComponent<IHackObject>(out var ho))
                    {
                        if (ho.canHackToolTag.Contains(hand.thisTool))
                        {
                            // 使えるカードは明るくなる
                            hand.isBlackOut = false;
                            hackObj = cameraPositionController.targetObject;
                        }
                    }
                }

                // 手を重ねると情報出る、位置とサイズ移動
                if (hand.isPointerOn == true)
                {

                    firstHandPos.y = 100;
                    hand.toScale = new Vector2(1.2f, 1.2f);
                    hand.isTextDisp = true;
                    hand.isBlackOut = true;

                    ramUIDisp.willUseRam = deckSystem.ReturnToolCost(hand.thisTool);
                    isHandOn = true;

                    // クリックするとカードプレイ
                    if (Input.GetKeyDown(KeyCode.Mouse1) && hackObj != null)
                    {
                        useableRam.UseRam(deckSystem.ReturnToolCost(hand.thisTool));

                        SeManager.Instance.Play("toolPlay");
                        toolTag playTool = deckSystem.HandPlay(i, hackObj);

                        trashToolUIList.Add(hand);
                        handToolUIList.RemoveAt(i);


                        //if (playTool != toolTag.none && deckSystem.RamUse(deckSystem.ReturnToolCost(hand.thisTool), (int)Player.Instance.nowRam))
                        //{
                        //    trashToolUIList.Add(hand);
                        //    handToolUIList.RemoveAt(i);
                        //}
                        //else
                        //{
                        //    Debug.LogError("おい！プレイできんかったぞ！");
                        //}
                    }
                }
                //else
                //{
                //    hand.toScale = new Vector2(1f, 1f);
                //    hand.isTextDisp = false;
                //    hand.isBlackOut = false;
                //}
            }

            hand.toMovePosition = nowHandPos + firstHandPos;

            //hand.thisTool = deckSystem.toolHand[i];
            hand.isOpen = true;
        }

        if (!isHandOn) ramUIDisp.willUseRam = 0;
    }

    void TrashControl()
    {
        // トラッシュチェックのオンオフ
        bool isTrashOn = false;
        for (int i = 0; i < trashToolUIList.Count; i++)
        {
            if (trashToolUIList[i].isPointerOn) isTrashOn = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
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

                trash.toScale = new Vector2(1f, 1f);
                trash.isTextDisp = false;
                trash.isBlackOut = false;

                trash.toMovePosition = nowTrashPos;
            }
        }
    }
}
