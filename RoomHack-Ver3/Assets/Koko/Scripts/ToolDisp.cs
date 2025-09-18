using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDisp : MonoBehaviour
{
    enum tool
    {
        none,
        CCTV,
        Bind,
        Silence,
        POT,
        Bomb
    }

    tool toolName = tool.none;

    int toolCost;

    string toolText;




}
