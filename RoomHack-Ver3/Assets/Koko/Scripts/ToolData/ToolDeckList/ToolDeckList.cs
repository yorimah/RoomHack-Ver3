using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ToolDeckList")]

public class ToolDeckList : ScriptableObject
{
    public List<tool> deckList = new List<tool>();
}

////System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
//[System.Serializable]
//public class ToolData
//{
//    public string toolName;
//    public int toolCost;
//    public string toolText;
//    public Sprite toolSprite;
//    public GameObject toolEvent;
//}