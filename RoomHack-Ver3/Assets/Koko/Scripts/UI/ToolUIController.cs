using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUIController : MonoBehaviour
{
    [SerializeField]
    DeckSystem deckSystem;

    [SerializeField]
    GameObject toolUIPrefab;

    [SerializeField]
    Vector2 deckPos = new Vector2(-800, 0);

    [SerializeField]
    Vector2 trashPos = new Vector2(-800, -320);

    [SerializeField]
    Vector2 handPos = new Vector2(0, -480);

    [SerializeField]
    float handSpace = 200;

    [SerializeField]
    List<GameObject> toolDeskUIList = new List<GameObject>();

    private void Update()
    {
        if (deckSystem.toolDesk.Count != toolDeskUIList.Count)
        {
            GameObject newToolUI = Instantiate(toolUIPrefab, deckPos, Quaternion.identity, this.transform);
            toolDeskUIList.Add(newToolUI);
        }

        for (int i = 0; i < toolDeskUIList.Count; i++)
        {
            float firstHandPos = (toolDeskUIList.Count - 1) * -(handSpace / 2);
            toolDeskUIList[i].GetComponent<ToolUI>().toMovePosition = handPos + new Vector2(firstHandPos + handSpace * i, 0);
            toolDeskUIList[i].GetComponent<ToolUI>().thisTool = deckSystem.toolDesk[i];
            toolDeskUIList[i].GetComponent<ToolUI>().isOpen = true;
        }
    }
}
