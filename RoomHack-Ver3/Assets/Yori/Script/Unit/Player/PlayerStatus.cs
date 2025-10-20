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
    // Ram回復系 
    public bool IsRebooting { get; private set; } = false;
    public float RebootTimer { get; private set; } = 0;

    public float MoveSpeed { get; }

    public List<int> deckList { get; private set; }

    public void setPlayerPosition(Transform transform)
    {
        PlayerPosition = transform.position;
    }
    public PlayerStatus()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        MaxHitPoint = saveData.maxHitPoint;
        nowHitPoint = MaxHitPoint;
        RamCapacity = saveData.maxRamCapacity;
        NowRam = RamCapacity;
        RamRecovary = saveData.RamRecovery;
        MoveSpeed = saveData.moveSpeed;

        deckList = saveData.deckList;
    }

    public void UseRam(int useRam)
    {
        NowRam -= useRam;
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
    public void setIsRebooting(bool setBool)
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

    public void setPlayerPosition(Transform transform);
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
    public void UseRam(int useRam);

    public void RamUpdate();

    public void setIsRebooting(bool setBoll);
}

public interface IDeckList
{
    public List<int> deckList { get; }
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