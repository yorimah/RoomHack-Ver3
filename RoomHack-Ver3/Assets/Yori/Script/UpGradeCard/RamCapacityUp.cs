using UnityEngine;

public class RamCapacityUp : MonoBehaviour, ICardType
{
    PlayerSaveData data;
    public int cardLevel { get; set; } = 1;
    public int cardWeight { get; set; } = 10;
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.plusRamCapacity += 1 * cardLevel;
        return data;
    }
}
