using UnityEngine;

public class MoveSpeedUp : MonoBehaviour, ICardType
{
    PlayerSaveData data;
    public int cardLevel { get; set; } = 1;
    public int cardWeight { get; set; } = 10;
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.plusMoveSpeed += 0.5f * cardLevel;
        return data;
    }
}
