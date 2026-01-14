using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "StageData")]
public class StageData : ScriptableObject
{
    [SerializeField]
    public string stageName;

    [SerializeField]
    public string stageExplain;

    [SerializeField]
    public int reward;

    [SerializeField]
    public int stageLevel;

    [SerializeField]
    public int floorNum;

    [SerializeField]
    public List<StagePartData> dataList = new List<StagePartData>();
}

[System.Serializable]
public class StagePartData
{
    public RandomFloorData randomFloorData;
    public int floorNo;
}

