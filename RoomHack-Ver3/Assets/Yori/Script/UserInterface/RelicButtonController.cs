using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class RelicButtonController : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    RelicDataBank relicDataBank;

    [SerializeField, Header("生成するボタン")]
    private RelicBuyButton insButtonObj;

    private RelicBuyButton insObj;

    private int buttonSpace = -200;

    private Vector3 initButtonPos = new Vector3(0, 500, 0);

    private List<RectTransform> insObjList = new List<RectTransform>();

    private RectTransform backGroundRect;

    [Inject]
    ISetMoneyNum setMoneyNum;

    [Inject]
    ISetRelicList setRelicList;
    public void Start()
    {
        backGroundRect = this.gameObject.GetComponent<RectTransform>();
        for (int i = 1; i < relicDataBank.relicDataList.Count; i++)
        {
            insObj = Instantiate(insButtonObj, initButtonPos, Quaternion.identity).GetComponent<RelicBuyButton>();
            insObjList.Add(insObj.gameObject.GetComponent<RectTransform>());
            insObj.transform.parent = this.transform;
            insObj.transform.localPosition = new Vector3(0, 500 + buttonSpace * i, 0);
            insObj.transform.localScale = new Vector3(1, 1, 1);
            insObj.SetRelicButton(relicDataBank.relicDataList[i].relicName, setMoneyNum,setRelicList);
            insObj.gameObject.name = relicDataBank.relicDataList[i].nameText + "Button";
        }
    }

    public void Update()
    {
        var scroll = Input.mouseScrollDelta.y * Time.deltaTime * 100;
        if (Mathf.Abs(scroll) > 1)
        {
            if (insObjList[insObjList.Count - 1].anchoredPosition.y - scroll * 10 <= backGroundRect.rect.height / 2 &&
                insObjList[0].anchoredPosition.y - scroll * 10 >= -backGroundRect.rect.height / 2)
            {
                foreach (var insObj in insObjList)
                {
                    insObj.localPosition += new Vector3(0, -scroll * 10, 0);
                }
            }
        }
    }

}
