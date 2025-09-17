using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHack : UnitCore
{
    // Start is called before the first frame update

    [SerializeField, Header("ぶりーちぱわー")]
    private float breachPower;

    [SerializeField, Header("VCamera")]
    private GameObject vCameraObj;
    private CinemachineVirtualCamera vCameraCM;

    [SerializeField, Header("ハック描画カメラ")]
    private GameObject hackCamera;
    private Rigidbody2D vCameraRB;
    void Start()
    {
        breachPower += data.plusBreachPower;
        vCameraRB = vCameraObj.GetComponent<Rigidbody2D>();
        vCameraCM = vCameraObj.GetComponent<CinemachineVirtualCamera>();
    }
    private void Hack()
    {
        // カメラを動かすように
        vCameraRB.velocity = PlayerMoveVector(moveInput.MoveValue(), moveSpeed - data.plusMoveSpeed);
        // 下方向にレイを飛ばす（距離10）
        RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.transform.position, new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
        foreach (RaycastHit2D hit in hitsss)
        {
            if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
            {
                if (vCameraRB.velocity == Vector2.zero)
                {
                    Vector3 enemyPos = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, vCameraCM.transform.position.z);
                    vCameraCM.transform.position = enemyPos;
                }
                if (Input.GetKeyDown(KeyCode.Mouse0) && !hackObject.clacked)
                {
                    Debug.Log("ハック挑戦！");
                    if (0 < UnitCore.Instance.nowRam)
                    {
                        UnitCore.Instance.nowRam--;
                        hackObject.Clack(breachPower);
                    }
                    else
                    {
                        // ハックできないときの処理
                        Debug.Log("ハックできないにょ～～～ん");
                    }
                }
            }
        }
    }
}
