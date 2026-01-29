using UnityEngine;
using System.Collections.Generic;

public class ToolUIController_MouseWheel : ToolUIController
{
    [SerializeField, Header("ToolTextDemoをあたっち")]
    GeneralUpdateText toolTextDemo;
    [SerializeField, Header("かりであたっち、きもすぎわろた")]
    ToolDataBank toolDataBank;

    protected override void SelectIndexCheck()
    {
        isHandOn = true;

        selectIndex -= (int)Input.mouseScrollDelta.y;
        if (selectIndex < 0) selectIndex = 0;
        if (selectIndex >= handToolUIList.Count) selectIndex = handToolUIList.Count - 1;

    }

    protected override void HandControl()
    {
        // ハンドのToolの位置と見た目
        // ハンドプレイ
        for (int i = 0; i < handToolUIList.Count; i++)
        {
            ToolUI hand = handToolUIList[i];

            // 初期設定
            Vector2 firstHandPos;
            firstHandPos.x = 0;
            //firstHandPos.y = ((handToolUIList.Count - 1) * -(handSpace / 2)) + handSpace * i;
            firstHandPos.y = -handSpace * (i - selectIndex) ;
            hand.toScale = handUIScale;
            hand.isTextDisp = false;
            hand.isBlackOut = true;

            // ツールコストが足りるかチェック
            if (!handCostList[i])
            {
                //firstHandPos.y = -100;
                //firstHandPos.x += 100;
                hand.isBlackOut = true;
            }
            else
            {
                if (handPlayList[i])
                {
                    hand.isBlackOut = false;
                }

                // 情報出る、位置とサイズ移動
                if (i == selectIndex)
                {
                    firstHandPos.x -= 100;
                    hand.toScale = handUIScale * 1.2f;
                    //hand.isTextDisp = true;

                    if (toolTextDemo != null)
                    {
                        toolTextDemo.inputText = toolDataBank.toolDataList[(int)hand.thisTool].toolText;
                    }
                }
            }

            hand.toMovePosition = nowHandPos + firstHandPos;

            hand.isOpen = true;
        }
    }
}
