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
    public ToolTag toolTag;
    public toolType toolType;
    public string toolName;
    public int toolCost;
    public string toolText;
    public Sprite toolSprite;
    public ToolEventBase toolEvent;
}


public enum ToolTag
{
    none,
    VisionHack,
    DeadRock,
    WeponGlitch,
    OverHeat,
    Detonation,
    BitClack,
    ResourceClip,
    LightResource,
    InstantMemory,
    FastReload,
    Armorust,
    Disruption,
    ClockReplication,
    RiotBurst,
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

public enum UnionTag
{
    none,
    Vandalers,
    Vigilante,
    NetSec,
}