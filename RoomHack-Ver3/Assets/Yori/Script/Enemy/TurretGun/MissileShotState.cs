using UnityEngine;
using Cysharp.Threading.Tasks;

public class MissileShotState : IState
{
    private Enemy enemy;
    enum ShotSection
    {
        aim,
        shot,
        shotInterval,
        sum
    }
    private ShotSection shotSection;

    private Rigidbody2D EnemyRigidBody2D;

    // 汎用タイマー
    private float timer;
    // Player情報
    private PlayerCheack playerCheack;



    [SerializeField, Header("ミサイルクールタイム")]
    float missileCoolTime = 5;
    public MissileShotState(Enemy _enemy)
    {
        enemy = _enemy;
        EnemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;
    }
    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
    }

    public async UniTask Execute()
    {
        await UniTask.Yield();
        //プレイヤー方向に向く
        playerCheack.RotationFoward(enemy.transform);
        //発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:
                if (enemy.aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                else
                {
                    EnemyRigidBody2D.linearVelocity = Vector2.zero;
                    timer += GameTimer.Instance.ScaledDeltaTime;
                }
                break;
            case ShotSection.shot:
                if (enemy.shotIntervalTime >= 100)
                {
                    shotSection++;
                }
                else
                {
                    // 射撃
                    MissileShot();

                    shotSection++;
                }
                break;
            case ShotSection.shotInterval:
                timer += GameTimer.Instance.ScaledDeltaTime;
                // プレイヤーが射線上にいたら射撃へ
                // いなかったら移動へ
                if (!playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
                {
                    enemy.ChangeState(StateType.Move);
                }
                else
                {
                    if (missileCoolTime <= timer)
                    {
                        shotSection = ShotSection.aim;
                    }
                }
                break;
            default:
                break;
        }
    }
    public void MissileShot()
    {
        GameObject bulletGameObject = Object.Instantiate(enemy.bulletObject, enemy.transform.position, Quaternion.identity);

        Missile bulletCore = bulletGameObject.GetComponent<Missile>();

        Vector2 shotDirection = Quaternion.Euler(0, 0, enemy.transform.eulerAngles.z) * Vector3.up;

        bulletCore.hitDamegeLayer = enemy.hitDamegeLayer;
        bulletCore.hitStop = 0.1f;
        bulletGameObject.transform.up = shotDirection;
    }
    public void Exit()
    {
    }
}