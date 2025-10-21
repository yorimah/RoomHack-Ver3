using UnityEngine;
using System.Collections.Generic;

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

    PlayerSaveData playerSaveData;

    bool isRebootHandStay = false;

    GameObject targetObject;

    private void Start()
    {
        playerSaveData = SaveManager.Instance.Load();
    }

    private void Update()
    {
        // reboot関連
        if (Player.Instance.isRebooting)
        {
            for (int i = 0; i < deckSystem.toolHand.Count; i++)
            {
                HandTrash(0);
            }
            isRebootHandStay = true;
        }
        else
        {
            // 手札再配置
            if (isRebootHandStay)
            {
                SeManager.Instance.Play("RebootEnd");
                if (deckSystem.toolHand.Count < playerSaveData.handNum /*deckSystem.handSize*/)
                {
                    DeckDraw();
                }
                else
                {
                    isRebootHandStay = false;
                    //Debug.Log("rebootEnd!");
                }
            }
        }

        // ハッキングモード
        if (Player.Instance.stateType == Player.StateType.Hack)
        {
            toolUIController.handCostList = handCostList;
            toolUIController.handPlayList = handPlayList;

            // 手札チェック
            for (int i = 0; i < deckSystem.toolHand.Count; i++)
            {
                toolTag hand = deckSystem.toolHand[i];

                // コストが足りるかチェック
                handCostList[i] = false;
                if (deckSystem.ReturnToolCost(hand) <= Player.Instance.nowRam)
                {
                    handCostList[i] = true;
                }


                // 対象取れるかチェック
                handPlayList[i] = false;
                if (cameraPositionController.targetObject != null)
                {
                    if (cameraPositionController.targetObject.TryGetComponent<IHackObject>(out var ho))
                    {
                        if (ho.canHackToolTag.Contains(hand))
                        {
                            handPlayList[i] = true;
                            targetObject = cameraPositionController.targetObject;
                        }
                    }
                }

                // resourceなら対象なしでもok
                if (deckSystem.ReturnToolType(hand) == toolType.Resource)
                {
                    handPlayList[i] = true;
                    targetObject = Player.Instance.gameObject;
                }

                // マウスがハンドのツールを選択しているなら
                if (toolUIController.isHandOn && toolUIController.handOnIndex == i)
                {
                    // 消費RAM表示
                    ramUIDisp.willUseRam = deckSystem.ReturnToolCost(hand);

                    // 右クリック入力
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        // プレイ可能ならGO
                        if (handCostList[i] && handPlayList[i])
                        {
                            deckSystem.RamUse(hand);
                            HandPlay(i, targetObject);
                        }
                    }
                }
                else
                {
                    ramUIDisp.willUseRam = 0;
                }

            }


        }

    }

    public void DeckDraw()
    {
        toolTag drawTool = deckSystem.DeckDraw();

        // デッキ切れチェック
        if (drawTool != toolTag.none)
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
        toolUIController.HandTrash(_index);
        SeManager.Instance.Play("toolPlay");
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

    // RAM回復
    public void RamAdd(float _num)
    {
        float ram = Player.Instance.nowRam + _num;
        // 上限、下限を超えないかチェック
        if (ram <= Player.Instance.ramCapacity && ram >= 0)
        {
            Player.Instance.nowRam = ram;
        }
        else
        {
            Debug.LogError("RamAddで上限、下限を超えました");
        }
    }

    public List<toolTag> GetDeckData()
    {
        return deckSystem.toolDeck;
    }

    public List<toolTag> GetHandData()
    {
        return deckSystem.toolHand;
    }

    public List<toolTag> GetTrashData()
    {
        return deckSystem.toolTrash;
    }
}
