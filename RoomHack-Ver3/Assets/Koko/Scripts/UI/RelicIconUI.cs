using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RelicIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("要アタッチ")]
    RelicDataBank relicDataBank;

    public RelicName thisRelic;

    // 表示系関連
    public bool isActive = false;
    public bool isOnpointerTextDisp = true;

    // 表示テキスト関連
    [SerializeField, Header("テキストの親オブジェクトをアタッチ")] GameObject textParent;
    [SerializeField, Header("nameTextをアタッチ")] GeneralUpdateText nameText;
    [SerializeField, Header("explainTextをアタッチ")] GeneralUpdateText explainText;
    [SerializeField, Header("otherTextをアタッチ")] GeneralUpdateText otherText;
    [SerializeField, Header("枠をアタッチ")] GameObject textFlame;

    // イメージ変数
    Image image;
    float transparency = 0.5f;

    // マウス反応関連
    public bool isPointerOn { get; private set; } = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        SeManager.Instance.Play("toolMove");
        isPointerOn = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SeManager.Instance.StopImmediately("toolMove");
        isPointerOn = false;
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        TextDisp();

        TransChange();
    }

    void TextDisp()
    {
        if (isOnpointerTextDisp && isPointerOn)
        {
            //textParent.GetComponent<RectTransform>().localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            nameText.isRunning = true;
            explainText.isRunning = true;
            otherText.isRunning = true;
            textFlame.SetActive(true);

            nameText.inputText = relicDataBank.relicDataList[(int)thisRelic].nameText;
            explainText.inputText = relicDataBank.relicDataList[(int)thisRelic].explainText;
            otherText.inputText = "stac : null";
        }
        else
        {
            nameText.isRunning = false;
            explainText.isRunning = false;
            otherText.isRunning = false;
            textFlame.SetActive(false);
        }
    }

    void TransChange()
    {
        image.sprite = relicDataBank.relicDataList[(int)thisRelic].iconImage;
        image.color = new Color(1, 1, 1, transparency);

        if (isActive)
        {
            transparency = 1f;
        }
        else
        {
            float alpha = transparency - 0.5f;
            transparency = 0.5f + (alpha * 0.95f);
        }
    }
}
