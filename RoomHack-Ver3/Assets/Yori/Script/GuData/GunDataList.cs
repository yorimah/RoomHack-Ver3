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
        foreach (var item in gunDataList)
        {
            if (item.GunName == gunName)
            {
                return item;
            }
        }
        Debug.LogError("そんな名前の銃はないよ！");
        return null;
    }
}
