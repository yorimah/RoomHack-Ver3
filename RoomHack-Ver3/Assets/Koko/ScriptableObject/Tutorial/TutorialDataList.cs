using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TutorialDataList")]
public class TutorialDataList : ScriptableObject
{
    public List<TutorialData> dataList = new List<TutorialData>();
}

[System.Serializable]
public class TutorialData
{
    public Vector2 muskPos;
    public Vector2 muskSize;

    public Vector2 textPos;
    public string explains;

    public float time;
}