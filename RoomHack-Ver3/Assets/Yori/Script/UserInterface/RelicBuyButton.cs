using UnityEngine;
using UnityEngine.UI;

public class RelicBuyButton : MonoBehaviour
{
    [SerializeField]
    private Text relicNameTextBox;

    [SerializeField]
    private Text relicExpTextBox;

    [SerializeField]
    private Image iconImage;

    private RelicData relicData;

    [SerializeField]
    private GameObject relicExplainObj;

    ISetMoneyNum setMoneyNum;
    ISetRelicList setRelicList;

    [SerializeField]
    private Vector2 initButtonPos = Vector2.zero;

    GameObject relicWindowObj;

    ISetWindowList addWindowList;
    public void SetRelicButton(RelicData _relicData, ISetMoneyNum _setMoneyNum, ISetRelicList _setRelicList, ISetWindowList _addWindoList)
    {
        addWindowList = _addWindoList;
        relicData = _relicData;
        setMoneyNum = _setMoneyNum;
        setRelicList = _setRelicList;
        relicNameTextBox = relicNameTextBox.GetComponent<Text>();
        relicExpTextBox = relicExpTextBox.GetComponent<Text>();
        iconImage = iconImage.GetComponent<Image>();
        relicNameTextBox.text = relicData.nameText;
        relicExpTextBox.text = relicData.explainText;
        iconImage.sprite = relicData.iconImage;
    }
    public void PushBuy()
    {
        if (setMoneyNum != null)
        {
            if (setMoneyNum.CanUseMoney(relicData.relicPrice))
            {
                setMoneyNum.UseMoney(relicData.relicPrice);
                setRelicList.AddRelic(relicData.relicName);
            }
        }
    }

    public void InsRelicExplainWindow()
    {
        if (relicWindowObj == null)
        {
            relicWindowObj = Instantiate(relicExplainObj, initButtonPos, Quaternion.identity);
            relicWindowObj.transform.parent = transform.root;
            relicWindowObj.transform.localPosition = Vector3.zero;
            relicWindowObj.transform.localScale = Vector3.one;
            relicWindowObj.name = relicData.nameText + " : RelicExplainWindow";
            RelicExplainWindow relicExplainWindow = relicWindowObj.GetComponent<RelicExplainWindow>();
            relicExplainWindow.SetRelicButton(relicData, setMoneyNum, setRelicList, addWindowList);
        }
    }
}
