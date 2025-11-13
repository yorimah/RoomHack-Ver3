using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ToolGetDataList")]
public class ToolGetDataList : ScriptableObject
{
    public List<ToolGetData> upGradeCardList = new List<ToolGetData>();
}

public class ToolGetData : MonoBehaviour
{



    [System.Serializable]
    public class UpGradeData
    {
        public ToolTag toolTag;
        public int toolWeight;
    }
}
