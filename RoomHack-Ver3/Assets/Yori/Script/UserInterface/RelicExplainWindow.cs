using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// レリックの詳細を表示するウィンドウを制御するスクリプト
/// </summary>
public class RelicExplainWindow : WindowSystem
{
    [SerializeField]
    private Image iconImage;

    private RelicData relicData;

    ISetMoneyNum setMoneyNum;
    ISetRelicList setRelicList;

    [SerializeField]
    private WindowMove windowMove;

    private ISetWindowList setWindowList;

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

    protected override void OnClosed()
    {
        base.OnClosed();
        setWindowList.RemoveWindowList(windowMove);
        Destroy(gameObject);
    }

    protected override void OnOpened()
    {
        base.OnOpened();
        RunningText();
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
