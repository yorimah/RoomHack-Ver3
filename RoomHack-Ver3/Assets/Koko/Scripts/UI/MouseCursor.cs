using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    TextMesh dispText;

    float time;

    [Inject]
    IHaveGun haveGun;

    [Inject]
    IGetGunData getGunData;

    GunData gunData;

    private void Start()
    {
        gunData  = getGunData.GetGunData(haveGun.GunName);
    }
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        this.transform.position = pos;

        time = gunData.ShotIntervalTime - haveGun.ShotTimer;

        if (dispText != null)
        {
            if (haveGun.ShotTimer > 0)
            {
                dispText.text = time.ToString("F2");
            }
            else
            {
                dispText.text = "ready";
            }
        }
    }
}
