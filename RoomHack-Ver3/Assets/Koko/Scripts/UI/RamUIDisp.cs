using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RamUIDisp : MonoBehaviour
{
    public int willUseRam = 0;

    [SerializeField]
    int maxRamCap;
    [SerializeField]
    int nowRamCap;

    [SerializeField]
    GameObject ramUIPrefab;

    [SerializeField]
    Sprite ramFill;
    [SerializeField]
    Sprite ramUsed;
    [SerializeField]
    Sprite ramWillUse;

    List<GameObject> ramUIList = new List<GameObject>();

    private void Start()
    {
        maxRamCap = (int)UnitCore.Instance.ramCapacity;
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

        nowRamCap = (int)UnitCore.Instance.nowRam;

        for (int i = 0; i < maxRamCap; i++)
        {
            if (nowRamCap > i)
            {
                if (nowRamCap - willUseRam <= i)
                {
                    ramUIList[i].GetComponent<Image>().sprite = ramWillUse;
                }
                else
                {
                    ramUIList[i].GetComponent<Image>().sprite = ramFill;
                }
            }
            else
            {
                ramUIList[i].GetComponent<Image>().sprite = ramUsed;
            }
        }

        // 現在のramの値が変わると
        //if (plRam != nowRamCap)
        //{
        //    nowRamCap = plRam;

        //    // 表示切替
        //    for (int i = 0; i < maxRamCap; i++)
        //    {
        //        if(i < nowRamCap)
        //        {
        //            ramUIList[i].transform.GetChild(0).gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            ramUIList[i].transform.GetChild(0).gameObject.SetActive(false);
        //        }
        //    }
        //}
    }
}
