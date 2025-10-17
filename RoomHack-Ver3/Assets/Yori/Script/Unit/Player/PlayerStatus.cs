using UnityEngine;

public class PlayerStatus : IReadOnlyMoveSpeed, IUseableRam
{
    public int MaxHitPoint { get; private set; }

    public float nowHitPoint;

    public float RamCapacity { get; }

    public float NowRam { get; private set; }

    public float RamRecovary { get; private set; }
    // Ram回復系 
    public bool isRebooting = false;
    public float rebootTimer { get; private set; } = 0;

    public float MoveSpeed { get; }

    public PlayerStatus()
    {
        PlayerSaveData saveData = SaveManager.Instance.Load();
        MaxHitPoint = saveData.maxHitPoint;
        nowHitPoint = MaxHitPoint;
        RamCapacity = saveData.maxRamCapacity;
        NowRam = RamCapacity;
        RamRecovary = saveData.RamRecovery;
        MoveSpeed = saveData.moveSpeed;
    }

    public void UseRam(int useRam)
    {
        NowRam -= useRam;
    }
}
public interface IReadPosition
{
    public Vector3 PlayerPosition { get; }
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
    public void UseRam(int useRam);
}
