using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// レリックの詳細を表示するウィンドウを制御するスクリプト
/// </summary>
public class RelicExplainWindow : MonoBehaviour
{
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

    [SerializeField]
    GeneralUpdateText relicNameUpdate;
    [SerializeField]    
    GeneralUpdateText relicExpUpdate;
    [SerializeField]
    GeneralUpdateText relicPriceUpdate;

    List<GeneralUpdateText> generalUpdateTexts = new();
    public void SetRelicButton(RelicData _relicData, ISetMoneyNum _setMoneyNum, ISetRelicList _setRelicList, ISetWindowList _setWindowList)
    {
        relicData = _relicData;
        setMoneyNum = _setMoneyNum;
        setRelicList = _setRelicList;
        iconImage.sprite = relicData.iconImage;
        setWindowList = _setWindowList;
        setWindowList.AddWindowList(windowMove);
    }

    void Start()
    {
        generalUpdateTexts.Add(relicNameUpdate);
        generalUpdateTexts.Add(relicExpUpdate);
        generalUpdateTexts.Add(relicPriceUpdate);
        foreach (var generalUpdateText in generalUpdateTexts)
        {
            generalUpdateText.delay = 3;
        }
        TextUpdateInit();
        RunningText();
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
        TextUpdateInit();
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

    public void TextUpdateInit()
    {
        foreach (var generalUpdateText in generalUpdateTexts)
        {
            generalUpdateText.inputText = " ";
            generalUpdateText.isRunning = false;
        }
    }

    public void RunningText()
    {
        relicNameUpdate.inputText = relicData.nameText;
        relicExpUpdate.inputText = relicData.explainText;
        relicPriceUpdate.inputText = "$ : " + relicData.relicPrice.ToString();
        foreach (var generalUpdateText in generalUpdateTexts)
        {
            generalUpdateText.isRunning = true;
        }
    }
}
