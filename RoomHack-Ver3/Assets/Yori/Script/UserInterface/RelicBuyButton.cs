using UnityEngine;
using UnityEngine.UI;
public class RelicBuyButton : WindowSystem
{
    [SerializeField]
    private Text relicNameTextBox;

    [SerializeField]
    private Text relicPriceTextBox;

    [SerializeField]
    private Text relicExplainTextBox;

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
    [SerializeField, Header("生成位置")]
    Vector2 popPostion;
    public void SetRelicButton(RelicData _relicData, ISetMoneyNum _setMoneyNum, ISetRelicList _setRelicList, ISetWindowList _addWindoList)
    {
        addWindowList = _addWindoList;
        relicData = _relicData;
        setMoneyNum = _setMoneyNum;
        setRelicList = _setRelicList;
        relicNameTextBox = relicNameTextBox.GetComponent<Text>();
        relicPriceTextBox = relicPriceTextBox.GetComponent<Text>();
        relicExplainTextBox = relicExplainTextBox.GetComponent<Text>();
        iconImage = iconImage.GetComponent<Image>();
        relicNameTextBox.text = relicData.nameText;
        relicExplainTextBox.text = relicData.explainText;
        relicPriceTextBox.text = relicData.relicPrice.ToString();
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
            InitWindow(relicWindowObj.GetComponent<RectTransform>());
            _ = relicExplainWindow.Open();
        }
        else
        {
            _ = relicWindowObj.GetComponent<RelicExplainWindow>().Open();
        }
    }
}
