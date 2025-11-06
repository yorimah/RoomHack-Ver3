using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GunDataListHolder : MonoBehaviour
{
    [SerializeField]
    private List<GunData> gunData = new List<GunData>();

    [Inject]
    ISetGunData setGunData;

    public void Awake()
    {
        setGunData.SetGunData(gunData);
    }
}

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

public interface IGetGunData
{
    public GunData GetGunData(GunName gunName);
}

public interface ISetGunData
{
    public void SetGunData(List<GunData> gunDataList);
}
