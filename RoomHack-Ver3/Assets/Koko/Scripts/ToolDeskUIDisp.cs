using System.Collections.Generic;
using UnityEngine;

public class ToolDeskUIDisp : MonoBehaviour
{
    [SerializeField]
    ToolDataBank toolDataBank;

    [SerializeField]
    int toolID;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(toolDataBank.toolDataList[toolID].toolEvent);
        }
    }
}
