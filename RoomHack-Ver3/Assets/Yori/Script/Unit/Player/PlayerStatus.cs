using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class PlayerStatus : IReadOnlyMoveSpeed, IUseableRam, IDeckList, IReadPosition, IGetPlayerDie, ISetPlayerDied,IReadMaxHitPoint
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

    public void SetPlayerPosition(Transform transform)
    {
        PlayerPosition = transform.position;
    }

    public PlayerStatus()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        MaxHitPoint = saveData.maxHitPoint;
        nowHitPoint = saveData.maxHitPoint;
        RamCapacity = saveData.maxRamCapacity;

        NowRam = RamCapacity;
        RamRecovary = saveData.RamRecovery;
        MaxHandSize = saveData.maxHandSize;

        MoveSpeed = saveData.moveSpeed;

        DeckList = saveData.deckList;

        IsReboot = true;

        _ = RamUpdate();
    }

    public void RamUse(float useRam)
    {
        NowRam -= useRam;
    }


    public void ChangeRam(float addRam)
    {
        float ram = NowRam + addRam;
        // 上限、下限を超えないかチェック
        if (ram <= RamCapacity && ram >= 0)
        {
            NowRam += ram;
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
            if (IsReboot)
            {
                NowRam = 0;
                RebootTimer += GameTimer.Instance.ScaledDeltaTime;

                if (RebootTimer >= RamRecovary)
                {
                    NowRam = RamCapacity;

                    RebootTimer = 0;
                    IsReboot = false;
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
public interface IReadPosition
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

public interface IGetIsHackMode
{
    public bool isHackMode { get; }
}