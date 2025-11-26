using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ToolManager : MonoBehaviour
{
    // Singletonパターン
    public static ToolManager Instance { get; private set; }
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

    [SerializeField, Header("アタッチされたし")]
    ToolUIController toolUIController;

    [SerializeField, Header("アタッチされたし")]
    DeckSystem deckSystem;

    [SerializeField, Header("アタッチされたし")]
    RamUIDisp ramUIDisp;

    [SerializeField, Header("アタッチされたし")]
    CameraPositionController cameraPositionController;

    List<bool> handCostList = new List<bool>();
    List<bool> handPlayList = new List<bool>();

    bool isRebootHandStay = false;

    GameObject targetObject;

    [Inject]
    IUseableRam useableRam;

    public delegate void ToolPlayAction();
    public ToolPlayAction toolPlayAction = () => { };


    [Inject]
    IPlayerInput playerInput;

    private void Start()
    {
        toolUIController.ToolUIControllerStart();

        for (int i = 0; i < useableRam.HandMaxSize; i++)
        {
            //Debug.Log("hikimasita");
            DeckDraw();
        }

    }

    private void Update()
    {
        // reboot関連
        // 廃止につきStartに移行
        //if (useableRam.IsReboot)
        //{
        //    Debug.Log("リブート中");

        //    for (int i = 0; i < deckSystem.toolHand.Count; i++)
        //    {
        //        HandTrash(0);
        //    }
        //    isRebootHandStay = true;
        //}
        //else
        //{
        //    Debug.Log("リブート中じゃないよ");

        //    // 手札再配置
        //    if (isRebootHandStay)
        //    {
        //        Debug.Log("手札くばるよ");

        //        //SeManager.Instance.Play("RebootEnd");
        //        if (deckSystem.toolHand.Count < useableRam.HandMaxSize)
        //        {
        //            DeckDraw();
        //        }
        //        else
        //        {
        //            isRebootHandStay = false;
        //            //Debug.Log("rebootEnd!");
        //        }
        //    }
        //}

        // ハッキングモード
        if (GameTimer.Instance.IsHackTime)
        {
            toolUIController.handCostList = handCostList;
            toolUIController.handPlayList = handPlayList;

            ramUIDisp.willUseRam = 0;

            // 手札チェック
            for (int i = 0; i < deckSystem.toolHand.Count; i++)
            {
                ToolTag hand = deckSystem.toolHand[i];

                // コストが足りるかチェック
                handCostList[i] = false;
                if (deckSystem.ReturnToolCost(hand) <= useableRam.RamNow)
                {
                    handCostList[i] = true;
                }


                // 対象取れるかチェック
                handPlayList[i] = false;
                if (cameraPositionController.targetObject != null)
                {
                    if (cameraPositionController.targetObject.TryGetComponent<IHackObject>(out var hackObj))
                    {
                        if (hackObj.canHackToolTag.Contains(hand))
                        {
                            handPlayList[i] = true;
                            targetObject = cameraPositionController.targetObject;
                        }
                    }
                }



                // 対象不要ツールならタゲなしでもプレイ可能
                if (!deckSystem.ReturnToolNeedTarget(hand))
                {
                    //Debug.Log(hand + "は対象不要");
                    handPlayList[i] = true;
                }

                // マウスがハンドのツールを選択しているなら
                if (toolUIController.isHandOn && toolUIController.handOnIndex == i)
                {
                    // 消費RAM表示
                    ramUIDisp.willUseRam = deckSystem.ReturnToolCost(hand);

                    //Debug.Log(ramUIDisp.willUseRam);

                    // 右クリック入力
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        // プレイ可能ならGO
                        if (handCostList[i] && handPlayList[i])
                        {
                            deckSystem.ToolCostUse(hand);
                            Debug.Log(i);
                            HandPlay(i, targetObject);
                            toolPlayAction();
                        }
                    }
                }
            }
        }

        toolUIController.ToolUIControllerUpdate();
    }

    public void DeckDraw()
    {
        ToolTag drawTool = deckSystem.DeckDraw();

        // デッキ切れチェック
        if (drawTool != ToolTag.none)
        {

            toolUIController.DeckDraw(drawTool);

            handCostList.Add(false);
            handPlayList.Add(false);
        }
        else
        {
            TrashRefresh();
            DeckDraw();
        }

    }

    // 手札のカードを使い、トラッシュする
    void HandPlay(int _index, GameObject _target)
    {
        deckSystem.HandPlay(_index, _target);
        HandTrash(_index);
        SeManager.Instance.Play("toolPlay");


    }

    public void ChangeRam(float changeRam)
    {
        useableRam.RamChange(changeRam);
    }

    // ハンドをトラッシュする
    public void HandTrash(int _index)
    {
        deckSystem.HandTrash(_index);
        toolUIController.HandTrash(_index);

        handCostList.RemoveAt(_index);
        handPlayList.RemoveAt(_index);
    }

    // トラッシュをデッキに戻す
    void TrashRefresh()
    {
        deckSystem.TrashRefresh();
        toolUIController.TrashRefresh();
    }

    public List<ToolTag> GetDeckData()
    {
        return deckSystem.toolDeck;
    }

    public List<ToolTag> GetHandData()
    {
        return deckSystem.toolHand;
    }

    public List<ToolTag> GetTrashData()
    {
        return deckSystem.toolTrash;
    }
}
