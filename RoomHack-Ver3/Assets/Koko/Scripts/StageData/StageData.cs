using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "StageData")]
public class StageData : ScriptableObject
{
    [SerializeField]
    public int stageNum;

    [SerializeField]
    public List<StagePartData> dataList = new List<StagePartData>();
}

[System.Serializable]
public class StagePartData
{
    public RandomFloorData randomFloorData;
    public int stageNo;
}

