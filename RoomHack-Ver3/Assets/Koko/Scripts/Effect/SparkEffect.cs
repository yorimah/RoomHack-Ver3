using System;
using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    public bool isAct = false;
    bool lateAct = false;

    public Vector2 startPos;
    public Vector2 endPos;

    [SerializeField]
    GameObject[] sparkList;

    [SerializeField]
    GameObject layerObject;

    private void Update()
    {
        if (isAct)
        {
            if (!lateAct)
            {
                this.transform.position = startPos;
                this.transform.localEulerAngles = endPos - startPos;

                lateAct = true;
            }

            for (int i = 0; i < sparkList.Length; i++)
            {

            }





        }
    }
}
