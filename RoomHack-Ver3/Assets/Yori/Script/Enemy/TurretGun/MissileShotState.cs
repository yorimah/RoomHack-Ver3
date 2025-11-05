using UnityEngine;

public class MissileShotState : IEnemyState
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

    Missile.Factory missileFactory;

    public MissileShotState(Enemy _enemy, Missile.Factory _MissileFactory)
    {
        enemy = _enemy;
        EnemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        missileFactory = _MissileFactory;

        // プレイヤー情報初期化
        playerCheack = enemy.playerCheack;
    }
    public void Enter()
    {
        shotSection = ShotSection.aim;
        timer = 0;
    }

    public void Execute()
    {
        //プレイヤー方向に向く
        playerCheack.RotationFoward(enemy.transform, enemy.PlayerPosition);
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
                    timer += GameTimer.Instance.GetScaledDeltaTime();
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
                timer += GameTimer.Instance.GetScaledDeltaTime();
                // プレイヤーが射線上にいたら射撃へ
                // いなかったら移動へ
                if (!playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
                {
                    enemy.ChangeState(EnemyStateType.Move);
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
        Vector2 shotDirection = Quaternion.Euler(0, 0, enemy.transform.eulerAngles.z) * Vector3.up;

        missileFactory.Create(0.1f, shotDirection, enemy.getIPosition, enemy.transform.position);
    }
    public void Exit()
    {
    }
}