using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(menuName = "ToolDeckList")]


// 使わない可能性あり
public class ToolDeckList : MonoBehaviour
{
    public List<ToolTag> deckList = new List<ToolTag>();
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