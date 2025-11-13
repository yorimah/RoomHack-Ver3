using UnityEngine;

public class WindowMove : MonoBehaviour
{
    Vector3 mouseStartPos;
    Vector3 cameraStartPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // マウスドラッグ初期設定
            mouseStartPos = Input.mousePosition;
            cameraStartPos = GetComponent<RectTransform>().position;

        }
        if (Input.GetKey(KeyCode.Mouse0))
        {

            Vector3 mouseVec = Input.mousePosition - mouseStartPos;
            Debug.Log(mouseVec);
            GetComponent<RectTransform>().position = cameraStartPos + mouseVec/100;
        }
    }

    GameObject GetMousePositionObject()
    {
        GameObject obj = null;

        // レイ射出
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            Debug.Log(hit.point);
        }

        return obj;
    }
}
