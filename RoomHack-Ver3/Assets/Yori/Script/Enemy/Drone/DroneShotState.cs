using UnityEngine;

public class DroneShotState : IState
{
    private Enemy enemy;

    private float flipTimer = 0;

    private float flipInterval = 1;

    private int direction = 1;
    private float moveDire;
    private Rigidbody2D enemyRigidBody2D;
    private PlayerCheack playerCheack;

    // ShotSection
    private float shotIntevalTime = 0;
    enum ShotSection
    {
        aim,
        shot,
        shotInterval,
        sum
    }
    private ShotSection shotSection;

    private BulletGeneratar bulletGeneratar;

    private float timer;
    public DroneShotState(Enemy _enemy)
    {
        enemy = _enemy;
        playerCheack = enemy.playerCheack;
        enemyRigidBody2D = enemy.GetComponent<Rigidbody2D>();

        bulletGeneratar = enemy.gameObject.GetComponent<BulletGeneratar>();


        shotIntevalTime = 1f;

    }
    public void Enter()
    {

    }

    public void Execute()
    {
        Debug.Log(shotSection);
        if (!playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
        {
            enemy.ChangeState(Enemy.StateType.Move);
        }
        Vector2 nowPosition = enemy.transform.position;
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            moveDire = Random.value < 0.5f ? -1 : 1;
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }
        Vector2 moveXY = moveDire < 0.5f ? new Vector2(1, 0) : new Vector2(0, 1);

        Vector2 emDir = moveXY;
        Vector2 nextPos = (nowPosition + (emDir * direction));

        // 障害物チェック
        Vector2 directionToNext = (nextPos - nowPosition).normalized;
        float checkDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(nowPosition, directionToNext, checkDistance, enemy.GetObstacleMask());
        if (hit.collider != null)
        {
            direction *= -1;
            moveDire = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
            enemyRigidBody2D.velocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRigidBody2D.velocity = (directionToNext.normalized * enemy.moveSpeed * GameTimer.Instance.ScaledDeltaTime) / 2;

        playerCheack.RotationFoward(enemy.transform);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:

                enemyRigidBody2D.velocity = Vector2.zero;
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (enemy.aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                bulletGeneratar.GunFire(enemy.bulletSpeed, enemy.HitDamegeLayer);
                shotSection++;
                break;
            case ShotSection.shotInterval:
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (shotIntevalTime <= timer)
                {
                    timer = 0;
                    shotSection = ShotSection.shot;
                }
                break;
            default:
                break;
        }
    }

    public void Exit()
    {

    }
}