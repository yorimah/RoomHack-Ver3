using UnityEngine;

public class PlayerHackState : IState
{
    private Player player;
    private PlayerMove playerMove;
    private PlayerShot playerShot;
    public PlayerHackState(Player _player)
    {
        player = _player;
        playerMove = new PlayerMove(_player);
        playerShot = new PlayerShot(_player);
    }
    public void Enter()
    {

    }
    public void Execute()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SeManager.Instance.StopImmediately("HackStart");
        //    SeManager.Instance.Play("HackExit");
        //    Player.Instance.isRebooting = true;
        //    unitCore.ChangeState(Player.StateType.Action);
        //}

        //playerMove.PlMove();
        //playerShot.Shot();

        GameTimer.Instance.SetTimeScale(0.01f);

        if (Input.GetKeyDown(KeyCode.Space)) Player.Instance.isRebooting = true;

        //if (Input.GetMouseButtonDown(1))
        //if (Input.anyKeyDown)
        if (Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.Mouse0))
        {
            SeManager.Instance.StopImmediately("HackStart");
            SeManager.Instance.Play("HackExit");
            //Player.Instance.isRebooting = true;
            player.ChangeState(Player.StateType.Action);
        }
    }

    public void Exit()
    {

    }
    // Start is called before the first frame update

    //[SerializeField, Header("ぶりーちぱわー")]
    //private float breachPower;

    //[SerializeField, Header("VCamera")]
    //private GameObject vCameraObj;
    //private CinemachineVirtualCamera vCameraCM;

    //[SerializeField, Header("ハック描画カメラ")]
    //private GameObject hackCamera;
    //private Rigidbody2D vCameraRB;
    //void Start()
    //{
    //    breachPower += data.plusBreachPower;
    //    vCameraRB = vCameraObj.GetComponent<Rigidbody2D>();
    //    vCameraCM = vCameraObj.GetComponent<CinemachineVirtualCamera>();
    //}
    //private void Hack()
    //{
    //    // カメラを動かすように
    //    vCameraRB.velocity = PlayerMoveVector(moveInput.MoveValue(), moveSpeed - data.plusMoveSpeed);
    //    // 下方向にレイを飛ばす（距離10）
    //    RaycastHit2D[] hitsss = Physics2D.BoxCastAll(Camera.main.transform.position, new Vector2(0.5f, 0.5f), 0f, Vector2.down, 0.1f);
    //    foreach (RaycastHit2D hit in hitsss)
    //    {
    //        if (hit.collider.gameObject.TryGetComponent<IHackObject>(out var hackObject))
    //        {
    //            if (vCameraRB.velocity == Vector2.zero)
    //            {
    //                Vector3 enemyPos = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, vCameraCM.transform.position.z);
    //                vCameraCM.transform.position = enemyPos;
    //            }
    //            if (Input.GetKeyDown(KeyCode.Mouse0) && !hackObject.clacked)
    //            {
    //                Debug.Log("ハック挑戦！");
    //                if (0 < UnitCore.Instance.nowRam)
    //                {
    //                    UnitCore.Instance.nowRam--;
    //                    hackObject.Clack(breachPower);
    //                }
    //                else
    //                {
    //                    // ハックできないときの処理
    //                    Debug.Log("ハックできないにょ～～～ん");
    //                }
    //            }
    //        }
    //    }
    //}
}
