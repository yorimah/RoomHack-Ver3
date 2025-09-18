using UnityEngine;

public class PlayerActionState : IState
{
    private UnitCore unitCore;
    private PlayerMove playerMove;
    private PlayerShot playerShot;
    public PlayerActionState(UnitCore _unitCore)
    {
        unitCore = _unitCore;
        playerMove = new PlayerMove(unitCore);
        playerShot = new PlayerShot(unitCore);
    }
    public void Enter()
    {

    }
    public void Execute()
    {
        playerMove.PlMove();
        playerShot.Shot();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            unitCore.ChangeState(UnitCore.StateType.Hack);
        }
    }

    public void Exit()
    {

    }
    //private void ShotState()
    //{
    //    //  銃を撃つモードはタイムスケールは１、ハックモードは1/10
    //    switch (shotMode)
    //    {
    //        case ShotMode.GunMode:
    //            PlayerRotation();
    //            playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), moveSpeed) * GameTimer.Instance.customTimeScale;

    //            if (NOWBULLET > 0)
    //            {
    //                Shot();
    //            }
    //            if (Input.GetKeyDown(KeyCode.R))
    //            {
    //                timer = 0;
    //                shotMode = ShotMode.ReloadMode;
    //            }

    //            if (Input.GetKeyDown(KeyCode.Tab))
    //            {
    //                GameTimer.Instance.SetTimeScale(0.1f);
    //                hackCamera.SetActive(true);
    //                nomalCamera.SetActive(false);
    //                shotMode = ShotMode.HackMode;
    //                vCameraCM.Follow = null;
    //                Debug.Log("切替" + shotMode);
    //            }

    //            break;
    //        case ShotMode.HackMode:
    //            // 徐々に減速
    //            playerRigidbody2D.velocity *= 0.95f * GameTimer.Instance.customTimeScale;

    //            // Hack();

    //            if (Input.GetKeyDown(KeyCode.Tab))
    //            {
    //                // カメラを元に戻して、followをプレイヤーへ
    //                hackCamera.SetActive(false);
    //                nomalCamera.SetActive(true);
    //                vCameraCM.Follow = this.gameObject.transform;
    //                // 銃モードへ切り替えて時間を元に戻す
    //                shotMode = ShotMode.GunMode;
    //                GameTimer.Instance.SetTimeScale(1);
    //                Debug.Log("切替" + shotMode);
    //            }
    //            break;
    //        case ShotMode.ReloadMode:
    //            PlayerRotation();
    //            playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), moveSpeed) * GameTimer.Instance.customTimeScale;

    //            timer += GameTimer.Instance.customTimeScale;
    //            if (reloadTime <= timer)
    //            {
    //                Reload();
    //                shotMode = ShotMode.GunMode;
    //            }

    //            break;
    //        default:
    //            Debug.LogError("範囲を出たわよ\n ShotMode:" + shotMode);
    //            break;
    //    }
    //}

    //private void GunFire()
    //{
    //    GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

    //    Rigidbody2D bulletRigit = bulletGameObject.GetComponent<Rigidbody2D>();

    //    BulletCore bulletCore = bulletGameObject.GetComponent<BulletCore>();

    //    bulletCore.power = 40;
    //    bulletCore.hitStop = 0.1f;
    //    bulletCore.HitDamegeLayer = UnitCore.Instance.HitDamegeLayer;

    //    Vector3 shootDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
    //    bulletRigit.velocity = shootDirection * bulletSpeed;
    //    bulletGameObject.transform.up = shootDirection;
    //}
    
}
