using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewTest : MonoBehaviour
{
    RaycastHit2D rh2d;

    [SerializeField]
    GameObject triangle;

    [SerializeField]
    float rayRot = 360;
    [SerializeField]
    int rayValue = 36;
    [SerializeField]
    float rayLen = 5;

    [SerializeField]
    LayerMask targetLm;

    [SerializeField]
    List<List<Vector2>> viewPointList = new List<List<Vector2>>();
    List<Vector2> Items = new List<Vector2>();


    private void Update()
    {
        Vector3 startPos = this.transform.position;
        float partRot = rayRot / (rayValue - 1);
        float nowRot = 0;

        for (int i = 0; i < rayValue; i++)
        {
            Vector3 rayPos = new Vector3(rayLen * Mathf.Cos(nowRot * Mathf.Deg2Rad), rayLen * Mathf.Sin(nowRot * Mathf.Deg2Rad), 0);
            RaycastHit2D result = Physics2D.Linecast(startPos, startPos + rayPos, targetLm);
            Debug.DrawLine(startPos, startPos + rayPos, color: Color.red);

            if (result.collider != null)
            {
                Items.Add(result.point);
            }
            else
            {
                if (Items != null)
                {
                    viewPointList.Add(Items);
                    Items.Clear();
                }
            }
            nowRot -= partRot;
        }


    }
}
