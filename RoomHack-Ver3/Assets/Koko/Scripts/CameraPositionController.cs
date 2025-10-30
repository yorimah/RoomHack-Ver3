using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    public GameObject targetObject;

    [SerializeField, Header("ターゲットを掴む強さ")]
    float targetOutNum = 100;

    Vector3 mouseStartPos;
    Vector3 cameraStartPos;

    //bool isStart = false;

    [SerializeField]
    LayerMask viewLayer;
    [SerializeField, Header("animationobj")]
    private GameObject tagetAnimObj;

    private GameObject insTargetAnimObj;

    [SerializeField, Header("playerObj")]
    private GameObject playerObject;
    private void Start()
    {

        insTargetAnimObj = Instantiate(tagetAnimObj);
        insTargetAnimObj.SetActive(false);
    }

    private void Update()
    {
        if (targetObject != null)
        {
            //this.transform.position = targetObject.transform.position;

            // 対象が視界外ならnull化
            if (targetObject.TryGetComponent<IHackObject>(out var hackObject))
            {
                if (!hackObject.CanHack) targetObject = null;
            }

            // ロックオン時仮アニメーション再生
            if (targetObject != playerObject.gameObject)
            {
                insTargetAnimObj.transform.position = targetObject.transform.position;
                insTargetAnimObj.SetActive(true);
            }
            else
            {
                insTargetAnimObj.SetActive(false);
            }

            //// デバッグするだけ
            //if (targetObject.TryGetComponent<IHackObject>(out var hackObject))
            //{
            //    string hoge = null;
            //    foreach (var item in hackObject.nowHackEvent)
            //    {
            //        hoge += item.name;
            //        hoge += ", ";
            //    }
            //    Debug.Log(hoge);
            //}

        }
        else
        {
            insTargetAnimObj.SetActive(false);
        }

        if (GameTimer.Instance.IsHackTime)
        {
            //ハックスタート時
            //if (!isStart)
            //{
            //    // タゲ取得
            //    if (GetMousePositionObject() != null)
            //    {
            //        targetObject = GetMousePositionObject();
            //    }

            //    isStart = true;

            //}


            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                // タゲ取得
                if (GetMousePositionObject() != null)
                {
                    targetObject = GetMousePositionObject();
                }

                // マウスドラッグ初期設定
                mouseStartPos = Input.mousePosition;
                cameraStartPos = this.transform.position;

            }

            //// カメラドラッグ
            //if (Input.GetKey(KeyCode.Mouse1))
            //{
            //    Vector3 mouseVec = Input.mousePosition - mouseStartPos;

            //    // ドラッグするとオブジェクト解除
            //    if (Mathf.Abs(mouseVec.x) > targetOutNum || Mathf.Abs(mouseVec.y) > targetOutNum)
            //    {
            //        targetObject = null;
            //    }

            //    if (targetObject == null)
            //    {
            //        // ハックオブジェがなかった場合のポジション処理
            //        this.transform.position = cameraStartPos - mouseVec / 100;
            //    }
            //}

        }
        else
        {
            // お試しでカメラ固定
            this.transform.position = Vector2.zero;
            targetObject = null;

            // カメラをプレイヤー追従
            //this.transform.position = playerObject.transform.position;
            //targetObject = playerObject;

            //isStart = false;
        }
    }

    GameObject GetMousePositionObject()
    {
        GameObject obj = null;

        // レイ射出、四角
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);

        foreach (RaycastHit2D hit in hitsss)
        {
            if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject)
                || hit.collider.gameObject == playerObject)
            {
                obj = hit.collider.gameObject;
            }
        }

        return obj;
    }
}
