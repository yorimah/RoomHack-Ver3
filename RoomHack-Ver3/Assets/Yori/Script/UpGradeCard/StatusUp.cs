using UnityEngine;

public class StatusUp : MonoBehaviour, ICardType
{
    enum StatusType
    {
        none,
        MaxRam,
        HandSize,
        HitPoint,
        MoveSpeed,
    }

    [SerializeField]
    StatusType statusType = StatusType.none;

    PlayerSaveData data;
    public int cardWeight { get; set; } = 10;
    public int cardLevel { get; set; } = 1;

    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;

        switch (statusType)
        {
            case StatusType.none:
                break;

            case StatusType.MaxRam:
                break;

            case StatusType.HandSize:
                break;

            case StatusType.HitPoint:
                break;

            case StatusType.MoveSpeed:
                break;
        }


        data.ramRecovery += 0.5f * cardLevel;
        return data;
    }
}
