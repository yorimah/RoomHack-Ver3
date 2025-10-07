using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSizeUp : MonoBehaviour,ICardType
{
    PlayerSaveData data;
    public int cardLevel { get; set; } = 1;
    public int cardWeight { get; set; } = 10;
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.plusRamRecovery += 0.5f * cardLevel;
        return data;
    }
}
