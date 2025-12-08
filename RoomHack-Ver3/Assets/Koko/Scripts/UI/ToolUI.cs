using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("仮でアタッチするンゴ、どっかにまとめときてえな")]
    ToolDataBank toolDataBank;

    public ToolTag thisTool;

    RectTransform rect;

    // 表裏系変数
    public bool isOpen = false;
    float reverseNum = -1;
    Image thisImage;

    // 移動変数
    public Vector2 toMovePosition;
    public bool isMove;

    // サイズ変数
    public Vector2 toScale = new Vector2(1, 1);

    // マウスポインターがのってるか否か
    public bool isPointerOn = false;


    // テキストUIアタッチ
    [SerializeField]Text nameText;
    [SerializeField]Text costText;
    [SerializeField]GeneralUpdateText effectText;

    // 効果表示変数
    bool isNameCostDisp = false;
    public bool isTextDisp = false;
    public bool isBlackOut = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 動いてない時だけ機能するように変更
        if (!isMove)
        {
            SeManager.Instance.Play("toolMove");
            isPointerOn = true;
        }
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        SeManager.Instance.StopImmediately("toolMove");
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

        ToolDisp();

        MoveProcess();

        ScaleProcess();


        nameText.gameObject.SetActive(isNameCostDisp);
        costText.gameObject.SetActive(isNameCostDisp);
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

            isNameCostDisp = true;

            nameText.text = toolDataBank.toolDataList[(int)thisTool].toolName;
            costText.text = toolDataBank.toolDataList[(int)thisTool].toolCost.ToString();
        }
        else
        {
            //Debug.Log("裏");
            thisImage.sprite = toolDataBank.toolDataList[(int)ToolTag.none].toolSprite;

            isNameCostDisp = false;
        }
    }

    void ToolDisp()
    {
        if (isTextDisp)
        {
            //isNameCostDisp = false;
            effectText.gameObject.SetActive(true);
            effectText.inputText = toolDataBank.toolDataList[(int)thisTool].toolText;
        }
        else
        {
            effectText.gameObject.SetActive(false);
        }

        if (isBlackOut)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }


    }

    void MoveProcess()
    {
        Vector2 moveVec = (toMovePosition - (Vector2)rect.localPosition) / 10;

        rect.localPosition += (Vector3)moveVec;

        // 移動してるか否か
        if (Mathf.Abs(moveVec.x) > 0.1f || Mathf.Abs(moveVec.y) > 0.1f)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    void ScaleProcess()
    {
        Vector2 scaleVec = (new Vector2(toScale.x * reverseNum, toScale.y) - (Vector2)rect.localScale) / 10;

        rect.localScale += (Vector3)scaleVec;
    }

}
