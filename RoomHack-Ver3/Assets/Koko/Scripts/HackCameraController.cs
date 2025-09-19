using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackCameraController : MonoBehaviour
{
    private void Update()
    {
        if (UnitCore.Instance.statetype == UnitCore.StateType.Hack)
        {
            Debug.Log("はっくなう");

            if (Input.GetMouseButtonDown(0))
            {
                // レイ射出、四角
                RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.transform.position, new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
                foreach (RaycastHit2D hit in hitsss)
                {
                    if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
                    {
                        this.transform.position = hit.collider.gameObject.transform.position;
                    }
                }




                Debug.Log("くりくなう");

                this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            this.transform.position = UnitCore.Instance.gameObject.transform.position;
        }
    }
}
