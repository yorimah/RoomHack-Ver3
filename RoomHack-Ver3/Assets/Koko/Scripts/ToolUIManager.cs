using UnityEngine;
using System.Collections.Generic;

public class ToolUIManager : MonoBehaviour
{
    [SerializeField]
    ToolUI ToolUIPrefab;

    List<ToolUI> toolUIDeck = new List<ToolUI>();
    List<ToolUI> toolUIHand = new List<ToolUI>();
    List<ToolUI> toolUITrash = new List<ToolUI>();

    private void Start()
    {
        
    }

    ToolUI GenerateToolUI(toolTag _generateTool)
    {
        return Instantiate(ToolUIPrefab);
    }
}
