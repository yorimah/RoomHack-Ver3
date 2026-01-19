using UnityEngine;
using UnityEngine.UI;
public class RelicExplainWindow : MonoBehaviour
{
    [SerializeField]
    private Text relicNameTextBox;

    [SerializeField]
    private Text relicExpTextBox;

    [SerializeField]
    private Text relicPriceTextBox;

    [SerializeField]
    private Image iconImage;

    private RelicData relicData;

    ISetMoneyNum setMoneyNum;
    ISetRelicList setRelicList;

    [SerializeField]
    private WindowMove windowMove;

    private ISetWindowList setWindowList;
    public void SetRelicButton(RelicData _relicData, ISetMoneyNum _setMoneyNum, ISetRelicList _setRelicList, ISetWindowList _setWindowList)
    {
        relicData = _relicData;
        setMoneyNum = _setMoneyNum;
        setRelicList = _setRelicList;
        relicNameTextBox.text = relicData.nameText;
        relicExpTextBox.text = relicData.explainText;
        relicPriceTextBox.text = "$ : " + relicData.relicPrice.ToString();
        iconImage.sprite = relicData.iconImage;
        setWindowList = _setWindowList;
        setWindowList.AddWindowList(windowMove);
    }

    void Start()
    {
        relicNameTextBox = relicNameTextBox.GetComponent<Text>();
        relicExpTextBox = relicExpTextBox.GetComponent<Text>();
        relicPriceTextBox = relicPriceTextBox.GetComponent<Text>();
        iconImage = iconImage.GetComponent<Image>();
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

    public void Exit()
    {
        setWindowList.RemoveWindowList(windowMove);
        Destroy(gameObject);
    }
}
