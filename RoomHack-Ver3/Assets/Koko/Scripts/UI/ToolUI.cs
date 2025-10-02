using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("仮でアタッチするンゴ、どっかにまとめときてえな")]
    ToolDataBank toolDataBank;

    public tool thisTool;

    RectTransform rect;

    // 表裏系変数
    public bool isOpen = false;
    float reverseNum = -1;
    Image thisImage;

    // 効果表示変数
    public bool isTextDisp;

    // 移動変数
    public Vector2 toMovePosition;

    // サイズ変数
    public Vector2 toScale = new Vector2(1, 1);

    // マウスポインターがのってるか否か
    public bool isPointerOn = false;


    // テキストUIアタッチ
    [SerializeField]Text nameText;
    [SerializeField]Text costText;
    [SerializeField]Text effectText;
    bool nameCostDisp = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOn = false;
    }

    private void Start()
    {
        thisImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        ReverseProcess();

        TextDisp();

        MoveProcess();

        ScaleProcess();


        nameText.gameObject.SetActive(nameCostDisp);
        costText.gameObject.SetActive(nameCostDisp);
    }

    void ReverseProcess()
    {
        if (isOpen)
        {
            reverseNum = 1;
        //    if (reverseNum < 1)
        //    {
        //        float num = (1 - reverseNum) / 10;
        //        reverseNum += num;
        //    }
        }
        else
        {
            reverseNum = -1;
        //    if (reverseNum > -1)
        //    {
        //        float num = (-1 - reverseNum) / 10;
        //        reverseNum += num;
        //    }
        }

        if (rect.localScale.x >= 0)
        {
            //Debug.Log("表");
            thisImage.sprite = toolDataBank.toolDataList[(int)thisTool].toolSprite;

            nameCostDisp = true;

            nameText.text = toolDataBank.toolDataList[(int)thisTool].toolName;
            costText.text = toolDataBank.toolDataList[(int)thisTool].toolCost.ToString();
        }
        else
        {
            //Debug.Log("裏");
            thisImage.sprite = toolDataBank.toolDataList[(int)tool.none].toolSprite;

            nameCostDisp = false;
        }
    }

    void TextDisp()
    {
        if (isTextDisp)
        {
            nameCostDisp = false;
            effectText.gameObject.SetActive(true);
            effectText.text = toolDataBank.toolDataList[(int)thisTool].toolText;
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            effectText.gameObject.SetActive(false);
            GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
    }

    void MoveProcess()
    {
        Vector2 moveVec = (toMovePosition - (Vector2)rect.localPosition) / 10;

        rect.localPosition += (Vector3)moveVec;
    }

    void ScaleProcess()
    {
        Vector2 scaleVec = (new Vector2(toScale.x * reverseNum, toScale.y) - (Vector2)rect.localScale) / 10;

        rect.localScale += (Vector3)scaleVec;
    }

}
