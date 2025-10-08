using UnityEngine;

public class RamRecovaryUp : MonoBehaviour, ICardType
{
    PlayerSaveData data;
    public int cardLevel { get; set; } = 3;
    public int cardWeight { get; set; } = 10;
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.plusRamRecovery += 0.5f * cardLevel;
        return data;
    }
}
