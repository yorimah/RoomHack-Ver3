using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStatus : IReadOnlyMoveSpeed, IUseableRam, IDeckList, IReadPosition, IGetPlayerDie, ISetPlayerDied
{
    public event Action PlayerDie = delegate { };
    public Vector3 PlayerPosition { get; private set; }
    public int MaxHitPoint { get; private set; }

    public float nowHitPoint;

    public float RamCapacity { get; }

    public float NowRam { get; private set; }

    public float RamRecovary { get; private set; }

    public bool IsRebooting { get; private set; } = false;

    public int MaxHandSize { get; private set; }

    public float RebootTimer { get; private set; } = 0;

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
    }

    public void UseRam(float useRam)
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
    public void RamUpdate()
    {
        if (IsRebooting)
        {
            NowRam = 0;
            RebootTimer += GameTimer.Instance.ScaledDeltaTime;

            if (RebootTimer >= RamRecovary)
            {
                NowRam = RamCapacity;

                RebootTimer = 0;
                IsRebooting = false;
            }
        }
    }
    public void SetIsRebooting(bool setBool)
    {
        IsRebooting = setBool;
    }

    public void SetDied()
    {
        PlayerDie();
    }


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

    public bool IsRebooting { get; }

    public float RebootTimer { get; }
    public void UseRam(float useRam);

    public void ChangeRam(float addRam);

    public int MaxHandSize { get; }

    public void RamUpdate();

    public void SetIsRebooting(bool setBoll);
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