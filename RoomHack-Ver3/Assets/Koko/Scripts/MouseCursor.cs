using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    [SerializeField]
    Text dispText;

    // ショットインターバル用変数
    float time;

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        this.transform.position = pos;

        if (dispText != null)
        {
            if (time > 0)
            {
                dispText.text = time.ToString("F2");
            }
            else
            {
                dispText.text = "ready";
            }
        }
    }
}
