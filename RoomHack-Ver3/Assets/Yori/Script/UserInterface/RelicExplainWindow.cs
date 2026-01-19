using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// レリックの詳細を表示するウィンドウを制御するスクリプト
/// </summary>
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
    private RectTransform windowRect;

    private float waitSeconds;
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
        windowRect = this.GetComponent<RectTransform>();
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

    public async void Exit()
    {
        await FadeOutWindow();
        setWindowList.RemoveWindowList(windowMove);
        Destroy(gameObject);
    }

    public async UniTask FadeOutWindow()
    {
        windowMove.ButtonSetActive(false);
        while (windowRect.rect.height > 10)
        {
            windowRect.sizeDelta -= new Vector2(0, Screen.height / 10);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        while (windowRect.rect.width > 10)
        {
            windowRect.sizeDelta -= new Vector2(Screen.width / 10, 0);
            await UniTask.WaitForSeconds(waitSeconds);
        }
        windowMove.ButtonSetActive(false);
    }
}
