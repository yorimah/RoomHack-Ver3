using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "RandomFloorData")]
public class RandomFloorData : ScriptableObject
{
    public List<OneFloorData> dataList = new List<OneFloorData>();
}

[System.Serializable]
public class OneFloorData
{
    public string floorScene;
    public int weight;
}
