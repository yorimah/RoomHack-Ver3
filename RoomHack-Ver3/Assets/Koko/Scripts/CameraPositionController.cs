using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField]
    public GameObject targetObject;

    [SerializeField, Header("ターゲットを掴む強さ")]
    float targetOutNum = 100;

    Vector3 mouseStartPos;
    Vector3 cameraStartPos;

    [SerializeField]
    LayerMask viewLayer;
    [SerializeField, Header("animationobj")]
    private GameObject tagetAnimObj;

    private GameObject insTagetAnimObj;
    private void Start()
    {
        targetObject = UnitCore.Instance.gameObject;
        insTagetAnimObj = Instantiate(tagetAnimObj);
        insTagetAnimObj.SetActive(false);
    }

    private void Update()
    {
        if (targetObject != null)
        {
            if (targetObject != UnitCore.Instance.gameObject)
            {
                insTagetAnimObj.transform.position = targetObject.transform.position;
                insTagetAnimObj.SetActive(true);
            }
            else
            {
                insTagetAnimObj.SetActive(false);
            }
            this.transform.position = targetObject.transform.position;
        }
        else 
        {
            insTagetAnimObj.SetActive(false);
        }

        if (UnitCore.Instance.statetype == UnitCore.StateType.Hack)
        {
            //Debug.Log("仮でタイマーいじってるからな");
            GameTimer.Instance.customTimeScale = 0.1f;

            //Debug.Log("はっくなう");

            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("くりくなう");

                //// タゲリセット
                //targetObject = null;

                // レイ射出、四角
                RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);

                //bool isView = false;
                //GameObject hackObj = new GameObject();

                foreach (RaycastHit2D hit in hitsss)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    // クリック付近にハックオブジェがある場合ターゲット

                    //if (hit.collider.gameObject.layer == viewLayer)
                    //{
                    //    isView = true;
                    //}

                    if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject)
                        || hit.collider.gameObject == UnitCore.Instance.gameObject)
                    {
                        targetObject = hit.collider.gameObject;
                        //hackObj = hit.collider.gameObject;
                    }
                }

                //if (isView) targetObject = hackObj;

                mouseStartPos = Input.mousePosition;
                cameraStartPos = this.transform.position;

                //this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mouseVec = Input.mousePosition - mouseStartPos;

                // ドラッグするとオブジェクト解除
                if (Mathf.Abs(mouseVec.x) > targetOutNum || Mathf.Abs(mouseVec.y) > targetOutNum)
                {
                    targetObject = null;
                }

                if (targetObject == null)
                {
                    // ハックオブジェがなかった場合のポジション処理
                    this.transform.position = cameraStartPos - mouseVec / 100;
                }
            }
        }
        else
        {

            //Debug.Log("仮でタイマーいじってるからな");
            GameTimer.Instance.customTimeScale = 1f;

            this.transform.position = UnitCore.Instance.gameObject.transform.position;
            targetObject = UnitCore.Instance.gameObject;
        }
    }
}
