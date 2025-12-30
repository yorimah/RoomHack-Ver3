using UnityEngine;
using UnityEngine.UI;

public class RelicBuyButtom : MonoBehaviour
{
    private RelicName relicName = RelicName.none;

    [SerializeField]
    private Text relicNameTextBox;

    [SerializeField]
    private Text relicExpTextBox;

    [SerializeField]
    private RelicDataBank dataBank;

    [SerializeField]
    private Image iconImage;

    private RelicData relicData;

    ISetMoneyNum setMoneyNum;
    ISetRelicList setRelicList;
    private bool wasRewrite;
    public void SetRelicButtom(RelicName _relicName, ISetMoneyNum _setMoneyNum, ISetRelicList _setRelicList)
    {
        relicData = dataBank.relicDataList[(int)_relicName];
        relicName = _relicName;
        setMoneyNum = _setMoneyNum;
        setRelicList = _setRelicList;
    }

    void Start()
    {
        wasRewrite = false;
        relicNameTextBox = relicNameTextBox.GetComponent<Text>();
        relicExpTextBox = relicExpTextBox.GetComponent<Text>();
        iconImage = iconImage.GetComponent<Image>();
    }

    void Update()
    {
        if (relicName != RelicName.none && !wasRewrite)
        {
            wasRewrite = true;
            relicNameTextBox.text = relicData.nameText;
            relicExpTextBox.text = relicData.explainText;
            iconImage.sprite = relicData.iconImage;
        }
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
}
