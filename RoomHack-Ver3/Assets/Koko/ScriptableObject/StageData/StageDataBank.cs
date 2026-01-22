using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "StageDataBank")]
public class StageDataBank : ScriptableObject
{
    [SerializeField]
    public List<StageData> dataList = new List<StageData>();
}
