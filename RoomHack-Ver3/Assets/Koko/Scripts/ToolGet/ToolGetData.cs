using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ToolGetDataList")]
public class ToolGetDataList : ScriptableObject
{
    public List<ToolGetData> dataList = new List<ToolGetData>();
}

[System.Serializable]
public struct ToolGetData
{
    public ToolTag toolTag;
    public int toolWeight;
}
