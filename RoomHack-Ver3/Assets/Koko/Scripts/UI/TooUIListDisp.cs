using UnityEngine;
using System.Collections.Generic;

public class TooUIListDisp : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    ToolUI toolUIPrefab;

    public Vector2 startPos = Vector2.zero;

    public bool isDisp = false;

    public List<ToolTag> inputToolList = new List<ToolTag>();


    List<ToolUI> toolUIList = new List<ToolUI>();


    [SerializeField]
    Vector2 basePos = Vector2.zero;

    [SerializeField]
    Vector2 spaceOffset = new Vector2(200, 400);

    [SerializeField]
    int horizonNum = 5;


    bool isGenerated = false;


    private void Update()
    {
        if (isDisp)
        {
            // プレハブ生成
            if (!isGenerated)
            {
                for (int i = 0; i < inputToolList.Count; i++)
                {
                    toolUIList.Add(ToolUIGenerate(startPos));
                }

                isGenerated = true;
            }

            for (int i = 0; i < toolUIList.Count; i++)
            {
                // ツール表示変更
                toolUIList[i].thisTool = inputToolList[i];
                toolUIList[i].isOpen = true;


                Vector2 pos = basePos;

                pos.x = 
                    basePos.x
                    - (horizonNum - 1) / 2 * spaceOffset.x
                    + i % horizonNum * spaceOffset.x;

                pos.y = basePos.y;

                toolUIList[i].toMovePosition = pos;
            }


        }
        else
        {
            for (int i = 0; i < toolUIList.Count; i++)
            {
                toolUIList[i].toMovePosition = startPos;
            }

            //isGenerated = false;
        }
    }

    ToolUI ToolUIGenerate(Vector2 _pos)
    {
        return Instantiate(toolUIPrefab, _pos, Quaternion.identity, this.transform);
    }
}
