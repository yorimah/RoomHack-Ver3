using System.Collections.Generic;
using UnityEngine;

public class GunDataList : ISetGunData, IGetGunData
{
    private List<GunData> gunDataList;

    public GunDataList()
    {
        gunDataList = new List<GunData>();
    }
    public void SetGunData(List<GunData> _gunDataList)
    {
        gunDataList = _gunDataList;
    }
    public GunData GetGunData(GunName gunName)
    {
        if (gunDataList.Count <= 0)
        {
            Debug.LogError("guDataListに数値が入ってないよ！");
            return null;
        }
        foreach (var gunData in gunDataList)
        {
            if (gunData.GunName == gunName)
            {
                return gunData;
            }
        }
        Debug.LogError("そんな名前の銃はないよ！");
        return null;
    }
}
