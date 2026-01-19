using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToolUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Header("仮でアタッチするンゴ、どっかにまとめときてえな")]
    ToolDataBank toolDataBank;

    [SerializeField, Header("ツール枠アタッチ")]
    List<Sprite> toolFlameList = new List<Sprite>();

    public ToolTag thisTool;

    RectTransform flameRect;

    // 表裏系変数
    public bool isOpen = false;
    float reverseNum = -1;
    Image flameImage;

    // アイコン変数
    [SerializeField, Header("アイコンオブジェクトをアタッチ")]
    GameObject toolIcon;
    Image iconImage;


    // 移動変数
    public Vector2 toMovePosition;
    public bool isMove;

    // サイズ変数
    public Vector2 toScale = new Vector2(1, 1);

    // マウスポインターがのってるか否か
    public bool isPointerOn = false;
    // 移動中でもマウス反応するか
    public bool isMovePointable = true;


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
        if (!isMove || isMovePointable)
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
        flameImage = GetComponent<Image>();
        flameRect = GetComponent<RectTransform>();

        iconImage = toolIcon.GetComponent<Image>();
    }

    private void Update()
    {
        ReverseProcess();

        ToolDisp();

        MoveProcess();

        ScaleProcess();


        nameText.gameObject.SetActive(isNameCostDisp);
        costText.gameObject.SetActive(isNameCostDisp);
        toolIcon.gameObject.SetActive(isNameCostDisp);
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

        if (flameRect.localScale.x >= 0)
        {
            //Debug.Log("表");
            flameImage.sprite = toolFlameList[(int)toolDataBank.toolDataList[(int)thisTool].toolType];
            iconImage.sprite = toolDataBank.toolDataList[(int)thisTool].toolIconSprite;

            isNameCostDisp = true;

            nameText.text = toolDataBank.toolDataList[(int)thisTool].toolName;
            costText.text = toolDataBank.toolDataList[(int)thisTool].toolCost.ToString();
        }
        else
        {
            //Debug.Log("裏");
            flameImage.sprite = toolFlameList[(int)toolDataBank.toolDataList[(int)ToolTag.none].toolType];

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
        Vector2 moveVec = (toMovePosition - (Vector2)flameRect.localPosition) / 10;

        flameRect.localPosition += (Vector3)moveVec;

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
        Vector2 scaleVec = (new Vector2(toScale.x * reverseNum, toScale.y) - (Vector2)flameRect.localScale) / 10;

        flameRect.localScale += (Vector3)scaleVec;
        //iconRect.localScale += (Vector3)scaleVec;
    }

}
