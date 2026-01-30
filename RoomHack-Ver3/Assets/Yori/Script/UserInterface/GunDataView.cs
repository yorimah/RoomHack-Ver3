using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class GunDataView : MonoBehaviour
{
    [Inject]
    IGetGunData getGunData;

    Text text;

    [Inject]
    IHaveGun haveGun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = "GunName " + getGunData.GetGunData(haveGun.GunName);
    }
}
