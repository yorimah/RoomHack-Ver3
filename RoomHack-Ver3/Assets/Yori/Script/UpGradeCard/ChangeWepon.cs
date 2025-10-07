using UnityEngine;

public class ChangeWepon : MonoBehaviour, ICardType
{
    [SerializeField, Header("銃の種類")]
    private GunNo gunNo;
    PlayerSaveData data;
    public int cardLevel { get; set; } = 1;
    public int cardWeight { get; set; } = 10;
    public PlayerSaveData Choiced(PlayerSaveData _data)
    {
        data = _data;
        data.gun = (int)gunNo;
        return data;
    }
}
