using System.Collections.Generic;
using UnityEngine;

public class ToolDeskUIDisp : MonoBehaviour
{
    [SerializeField]
    ToolDataBank toolDataBank;

    int toolCost;

    private void Start()
    {
        Debug.Log(toolDataBank.toolDataList[0].toolName);
        Debug.Log(toolDataBank.toolDataList[0].toolCost);
        Debug.Log(toolDataBank.toolDataList[0].toolText);
    }
}
