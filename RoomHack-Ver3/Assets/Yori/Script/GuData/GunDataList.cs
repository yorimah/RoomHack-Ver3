using UnityEngine;
using System.Collections.Generic;

public class GunDataList : MonoBehaviour, IGunData
{
    [SerializeField]
    private List<GunData> gunData = new List<GunData>();

    public GunData GetGunData(GunName gunName)
    {
        foreach (var item in gunData)
        {
            if (item.GunName == gunName)
            {
                return item;
            }           
        }
        return null;
    }
}

public interface IGunData
{
    public GunData GetGunData(GunName gunName);
}
