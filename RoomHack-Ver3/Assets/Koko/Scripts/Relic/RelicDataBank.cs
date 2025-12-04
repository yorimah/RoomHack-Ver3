using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "RelicData")]
public class RelicDataBank : ScriptableObject
{
    public List<RelicData> relicDataList = new List<RelicData>();
}

[System.Serializable]
public class RelicData
{
    public RelicName relicName;
    public string nameText;
    public string explainText;
    public Sprite iconImage;
}
