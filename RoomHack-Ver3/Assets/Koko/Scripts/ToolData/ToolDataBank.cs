using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ToolData")]

public class ToolDataBank : ScriptableObject
{
    public List<ToolData> toolDataList = new List<ToolData>();
}

//System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
[System.Serializable]
public class ToolData
{
    public string toolName;
    public int toolCost;
    public string toolText;
    public Sprite toolSprite;
    public GameObject toolEvent;
}

public enum tool
{
    none,
    CCTVHack,
    Bind,
    Blind,
    OverHeat,
    Detonation
}