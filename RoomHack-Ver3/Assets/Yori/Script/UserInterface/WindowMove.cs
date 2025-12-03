using UnityEngine;

public class WindowMove : MonoBehaviour
{
    Vector3 mouseStartPos;
    Vector3 cameraStartPos;
    public bool canDrag;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // マウスドラッグ初期設定
            mouseStartPos = Input.mousePosition;
            cameraStartPos = GetComponent<RectTransform>().position;
            GetMousePositionObject();
        }
        if (Input.GetKey(KeyCode.Mouse0) & canDrag)
        {
            Vector3 mouseVec = Input.mousePosition - mouseStartPos;
            GetComponent<RectTransform>().position = cameraStartPos + mouseVec / 100;
        }
        else
        {
            canDrag = false;
        }
    }

    private void GetMousePositionObject()
    {
        // レイ射出
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        
        foreach (RaycastHit2D hit in hitsss)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.TryGetComponent<WindowMove>(out var moveWindow))
            {
                moveWindow.canDrag = true;
            }
        }
    }
}
