using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class RamUIDisp : MonoBehaviour
{
    [SerializeField]
    Vector2 defaultPos = new Vector2(0, -200);

    [SerializeField]
    Vector2 space = new Vector2(50, 0);

    public int willUseRam = 0;

    [SerializeField]
    int maxRamCap;
    [SerializeField]
    int nowRamCap;

    //public bool isSkelton = false;

    [SerializeField]
    GameObject ramUIPrefab;

    [SerializeField]
    Sprite ramFill;
    [SerializeField]
    Sprite ramUsed;
    [SerializeField]
    Sprite ramWillUse;

    [SerializeField]
    GeneralUpdateText ramText;

    List<GameObject> ramUIList = new List<GameObject>();

    [Inject]
    IUseableRam useableRam;

    private void Start()
    {
        maxRamCap = (int)useableRam.RamCapacity;
        // rmaxRamCapの値だけUI生成
        for (int i = 0; i < maxRamCap; i++)
        {
            GameObject ramObj = Instantiate(ramUIPrefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
            
            ramUIList.Add(ramObj);

            Vector3 rot = ramObj.GetComponent<RectTransform>().localEulerAngles;
            rot.z = 90;
            ramObj.GetComponent<RectTransform>().localEulerAngles = rot;


        }

    }

    private void Update()
    {

        nowRamCap = (int)useableRam.RamNow;

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

        for (int i = 0; i < ramUIList.Count; i++)
        {
            // hackモード時に半透明化
            //if (isSkelton)
            //if (GameTimer.Instance.IsHackTime)
            {
                ramUIList[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            //else
            //{
            //    ramUIList[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            //}

            // 位置設定
            Vector2 ramPos = defaultPos + new Vector2((maxRamCap - 1) * -space.x / 2 + i * space.x, (maxRamCap - 1) * -space.y / 2 + i * space.y);
            ramUIList[i].GetComponent<RectTransform>().anchoredPosition = ramPos;
        }

        //現在のramの値が変わると
        if (useableRam.RamNow != nowRamCap)
        {
            nowRamCap = (int)useableRam.RamNow;

            // 表示切替
            for (int i = 0; i < maxRamCap; i++)
            {
                if (i < nowRamCap)
                {
                    ramUIList[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    ramUIList[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        //ramText.inputText = nowRamCap.ToString("00");
    }
}
