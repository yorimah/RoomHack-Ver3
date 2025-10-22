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
    public toolTag toolTag;
    public toolType toolType;
    public string toolName;
    public int toolCost;
    public string toolText;
    public Sprite toolSprite;
    public ToolEvent toolEvent;
}






public enum toolTag
{
    none,
    CCTVHack,
    Bind,
    Blind,
    OverHeat,
    Detonation,
    EdgeRun,
    Blink,
    ResourceClip,
    LightResource,
    FastReload,
    InstantMemory,
    MemoryDeposit,
    Accelerate,
}

public enum toolType
{
    none,
    Attack,
    Buff,
    Debuff,
    Resource,
    Special
}