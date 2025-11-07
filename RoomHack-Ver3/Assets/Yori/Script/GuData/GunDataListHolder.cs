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
