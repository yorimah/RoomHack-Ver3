using UnityEngine;

public class PlayerStatus : IReadOnlyPlayerStatus
{
    public int MaxHitPoint { get; private set; }
    public float nowHitPoint;
    // ハックデータ
    public float ramCapacity;
    public float nowRam;
    public float ramRecovary;
    // Ram回復系 
    public bool isRebooting = false;
    public float rebootTimer { get; private set; } = 0;
    public  PlayerStatus(PlayerSaveData saveData)
    {
        MaxHitPoint = saveData.maxHitPoint;
        nowHitPoint = MaxHitPoint;

        ramCapacity = saveData.maxRamCapacity;
        nowRam = ramCapacity;
        ramRecovary = saveData.RamRecovery;
    }
}

public interface IReadOnlyPlayerStatus
{
    public int MaxHitPoint { get; }

    public int ReadMaxHitPoint() => MaxHitPoint;
}
public interface IReadOnlyPlayerPoision
{
    public Vector3 PlayerPosition { get; }

    public Vector3 ReadPlayerPosition() => PlayerPosition;
}
