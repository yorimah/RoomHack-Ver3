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
    public ToolDataBank toolDataBank;

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
    public IUseableRam useableRam;

    public delegate void ToolPlayAction();
    public ToolPlayAction toolPlayAction = () => { };


    [Inject]
    IPlayerInput playerInput;

    private void Start()
    {
        deckSystem.toolDataBank = toolDataBank;
        deckSystem.DeckSystemStart();

        toolUIController.ToolUIControllerStart();

        for (int i = 0; i < useableRam.HandMaxSize; i++)
        {
            //Debug.Log("hikimasita");
            DeckDraw();
        }

    }

    private void Update()
    {
        // ゲームが動いてるか否か
        if (GameTimer.Instance.playTime == 0) return;

        // ハッキング
        //if (GameTimer.Instance.playTime == 1)
        {
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

                // 対象取れてるかチェック
                handPlayList[i] = false;
                if (cameraPositionController.targetObject != null)
                {
                    if (cameraPositionController.targetObject.TryGetComponent<IHackObject>(out var hackObj))
                    {

                        if (!hackObj.cantHackToolType.Contains(toolDataBank.toolDataList[(int)hand].toolType))
                        {
                            targetObject = cameraPositionController.targetObject;
                            handPlayList[i] = true;
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
                if (toolUIController.isHandOn && toolUIController.selectIndex == i)
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
                            //Debug.Log(i);
                            HandPlay(i, targetObject);
                            toolPlayAction();
                        }
                    }
                }
            }

            // 手札チェックの結果をToolUIControllerに入力
            toolUIController.handCostList = handCostList;
            toolUIController.handPlayList = handPlayList;
        }
        
        if (!GameTimer.Instance.IsHackTime)
        {
            ramUIDisp.willUseRam = 0;
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

    // トラッシュをハンドへ
    public void TrashToHand(int _index)
    {
        deckSystem.ToolMove(deckSystem.toolTrash, _index, deckSystem.toolHand);
        toolUIController.TrashToHand(_index);

        handCostList.Add(false);
        handPlayList.Add(false);
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
