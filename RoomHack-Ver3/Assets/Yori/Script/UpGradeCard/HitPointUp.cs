using UnityEngine;

public class HitPointUp : MonoBehaviour, ICardType
{
    PlayerSaveData data;
    public int cardLevel { get; set; } = 1;
    public int cardWeight { get; set; } = 10;
    public void Start()
    {

    }
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.maxHitPoint += 10 * cardLevel;
        return data;
    }
}
