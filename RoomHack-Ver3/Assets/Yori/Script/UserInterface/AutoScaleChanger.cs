using System.Collections.Generic;
using UnityEngine;

public class AutoScaleChanger : MonoBehaviour
{
    [SerializeField, Header("サイズを調整したいobj")]
    private List<RectTransform> targetRectTransformList = new List<RectTransform>();

    private RectTransform thisRectTransform;
    public void Start()
    {
        thisRectTransform = GetComponent<RectTransform>();
    }

    public void Update()
    {
        foreach (var targetRect in targetRectTransformList)
        {
            if (targetRect.sizeDelta != thisRectTransform.sizeDelta)
            {
                targetRect.sizeDelta = thisRectTransform.sizeDelta;
            }
        }
    }
}
