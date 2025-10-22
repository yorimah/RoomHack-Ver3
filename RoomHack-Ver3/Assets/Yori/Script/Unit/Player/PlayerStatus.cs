using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

enum SpecialAction
{
    none,
    EdgeRun,
    Blink
}

public class PlayerStatus : IReadOnlyMoveSpeed, IUseableRam, IDeckList, IPosition,
    IGetPlayerDie, ISetPlayerDied, IReadMaxHitPoint
{
    public event Action PlayerDie = delegate { };

    public Vector3 PlayerPosition { get; private set; }

    public int MaxHitPoint { get; private set; }

    public float nowHitPoint;

    public float RamCapacity { get; }

    public float NowRam { get; private set; }

    public float RamRecovary { get; private set; }

    public bool IsReboot { get; private set; } = false;

    public int MaxHandSize { get; private set; }

    public float RebootTimer { get; private set; }

    public float MoveSpeed { get; }

    public List<int> DeckList { get; private set; }

    private SpecialAction SpecialAction = SpecialAction.none;

    public float specialActionCount = 0;


    public void SetPlayerPosition(Transform transform)
    {
        PlayerPosition = transform.position;
    }

    public PlayerStatus()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();

        // HP初期化
        MaxHitPoint = saveData.maxHitPoint;
        nowHitPoint = saveData.maxHitPoint;

        // Hack関連初期化
        RamCapacity = saveData.maxRamCapacity;
        NowRam = saveData.maxRamCapacity; ;
        RamRecovary = saveData.ramRecovery;
        MaxHandSize = saveData.maxHandSize;
        IsReboot = true;
        _ = RamUpdate();
        DeckList = saveData.deckList;

        MoveSpeed = saveData.moveSpeed;
    }

    public void RamUse(float useRam)
    {
        if (0 >= NowRam - useRam)
        {
            NowRam = 0;
            Debug.LogError("使える数より多いRAMを使おうとしてるよ！");
        }
        else
        {
            NowRam -= useRam;
        }
    }

    public void ChangeRam(float addRam)
    {
        float ram = NowRam + addRam;
        // 上限、下限を超えないかチェック
        if (ram <= RamCapacity && ram >= 0)
        {
            NowRam += addRam;
        }
        else
        {
            Debug.LogError("RamAddで上限、下限を超えました");
        }
    }

    public async UniTask RamUpdate()
    {
        while (true)
        {
            // IsRebootがtrueならRamRecovaryの時間まで待機して、Ramを初期値に戻す。
            if (IsReboot)
            {
                NowRam = 0;
                RebootTimer += GameTimer.Instance.GetScaledDeltaTime();

                if (RebootTimer >= RamRecovary)
                {
                    NowRam = RamCapacity;

                    RebootTimer = 0;
                    SetIsReboot(false);
                }
            }
            await UniTask.Yield();
        }
    }
    public void SetIsReboot(bool setBool)
    {
        IsReboot = setBool;
    }

    public void SetDied()
    {
        PlayerDie();
    }
}

public interface IReadMaxHitPoint
{
    public int MaxHitPoint { get; }
}
public interface IPosition
{
    public Vector3 PlayerPosition { get; }

    public void SetPlayerPosition(Transform transform);
}

public interface IReadOnlyMoveSpeed
{
    public float MoveSpeed { get; }
}

public interface IUseableRam
{
    public float RamCapacity { get; }

    public float NowRam { get; }

    public float RamRecovary { get; }

    public bool IsReboot { get; }

    public float RebootTimer { get; }
    public void RamUse(float useRam);

    public void ChangeRam(float addRam);

    public int MaxHandSize { get; }

    public UniTask RamUpdate();

    public void SetIsReboot(bool setBoll);
}

public interface IDeckList
{
    public List<int> DeckList { get; }
}

public interface IGetHelth
{
    public float GetMaxHitPoint();
    public float GetNowHitPoint();
}

public interface IGetPlayerDie
{
    public event Action PlayerDie;
}

public interface ISetPlayerDied
{
    public void SetDied();
}