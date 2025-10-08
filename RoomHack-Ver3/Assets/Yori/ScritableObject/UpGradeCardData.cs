using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "UpGradeData")]
public class UpGradeCardData : ScriptableObject
{
    public List<UpGradeData> toolDataList = new List<UpGradeData>();
}

[System.Serializable]
public class UpGradeData
{
    public UpGradeTag upGradeTag;
    public int CardLevel;
    public int CardWeight;
    public GameObject cardType;
}

public enum UpGradeTag
{
    MaxRum,
    RamRecover,
    HandSize,
    AddToolCCTVHack,
    AddToolBind,
    AddToolBlind,
    AddToolOverHeat,
    AddToolDetonation,
    RemoveTool,
    WeponChangeSniper,
    WeponChangeAssult,
    WeponChangeSubMachine,
    HitPointUP,
    MoveSpeedUp,
}
