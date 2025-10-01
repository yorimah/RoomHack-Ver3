using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    GameObject targetObject;

    Vector3 mouseStartPos;
    Vector3 cameraStartPos;

    private void Start()
    {
        targetObject = UnitCore.Instance.gameObject;
    }

    private void Update()
    {
        if (targetObject != null)
        {
            this.transform.position = targetObject.transform.position;
        }

        if (UnitCore.Instance.statetype == UnitCore.StateType.Hack)
        {
            Debug.Log("はっくなう");

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("くりくなう");

                // タゲリセット
                targetObject = null;

                // レイ射出、四角
                RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
                foreach (RaycastHit2D hit in hitsss)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    // クリック付近にハックオブジェがある場合ターゲット
                    if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject)
                        || hit.collider.gameObject == UnitCore.Instance.gameObject)
                    {
                        targetObject = hit.collider.gameObject;
                    }
                }

                // ハックオブジェがなかった場合の初期設定
                if (targetObject == null)
                {
                    mouseStartPos = Input.mousePosition;
                    cameraStartPos = this.transform.position;
                }

                //this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                if (targetObject == null)
                {
                    // ハックオブジェがなかった場合のポジション処理
                    this.transform.position = cameraStartPos - (Input.mousePosition - mouseStartPos) / 100;
                }
            }
        }
        else
        {
            this.transform.position = UnitCore.Instance.gameObject.transform.position;
            targetObject = UnitCore.Instance.gameObject;
        }
    }
}
