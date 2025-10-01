using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    [SerializeField, Header("仮でアタッチするンゴ、どっかにまとめときてえな")]
    ToolDataBank toolDataBank;

    public tool thisTool;

    // 表裏系変数
    public bool isOpen = false;
    float reverseNum = -1;
    Image thisImage;

    // 移動変数
    public Vector2 toMovePosition;
    RectTransform rect;

    [SerializeField]Text nameText;
    [SerializeField]Text costText;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        ReverseProcess();

        MoveProcess();

    }

    void ReverseProcess()
    {
        if (isOpen)
        {
            if (reverseNum < 1)
            {
                float num = (1 - reverseNum) / 10;
                reverseNum += num;
            }
        }
        else
        {
            if (reverseNum > -1)
            {
                float num = (-1 - reverseNum) / 10;
                reverseNum += num;
            }
        }

        if (reverseNum >= 0)
        {
            //Debug.Log("表");
            thisImage.sprite = toolDataBank.toolDataList[(int)thisTool].toolSprite;

            nameText.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);

            nameText.text = toolDataBank.toolDataList[(int)thisTool].toolName;
            costText.text = toolDataBank.toolDataList[(int)thisTool].toolCost.ToString();
        }
        else
        {
            //Debug.Log("裏");
            thisImage.sprite = toolDataBank.toolDataList[(int)tool.none].toolSprite;

            nameText.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
        }

        Vector2 scale = this.transform.localScale;
        scale.x = Mathf.Abs(reverseNum);
        this.transform.localScale = scale;
    }

    void MoveProcess()
    {
        Vector2 moveVec = (toMovePosition - (Vector2)rect.localPosition) / 10;

        rect.localPosition += (Vector3)moveVec;
    }
}
