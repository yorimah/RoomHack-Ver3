using UnityEngine;
using System.Collections.Generic;

public class RamUIDisp : MonoBehaviour
{
    [SerializeField]
    int maxRamCap;
    [SerializeField]
    int nowRamCap;

    [SerializeField]
    GameObject ramUIPrefab;

    [SerializeField]
    List<GameObject> ramUIList = new List<GameObject>();

    [SerializeField]
    int plRam = 1;

    private void Start()
    {
        // rmaxRamCapの値だけUI生成
        for (int i = 0; i < maxRamCap; i++)
        {
            GameObject ramObj = Instantiate(ramUIPrefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
            ramUIList.Add(ramObj);

            // 位置設定
            Vector2 ramPos = new Vector2((maxRamCap - 1) * -25 + i * 50, -200);
            ramObj.GetComponent<RectTransform>().anchoredPosition = ramPos;
        }

    }

    private void Update()
    {

        //plRam = UnitCore.Instance.ramCap;

        // 現在のramの値が変わると
        if (plRam != nowRamCap)
        {
            nowRamCap = plRam;

            // 表示切替
            for (int i = 0; i < maxRamCap; i++)
            {
                if(i < nowRamCap)
                {
                    ramUIList[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    ramUIList[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
