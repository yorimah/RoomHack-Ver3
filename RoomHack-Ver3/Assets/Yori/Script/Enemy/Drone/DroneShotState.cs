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

    }
    public void Enter()
    {

    }

    public void Execute()
    {
        if (!playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
        {
            enemy.ChangeState(StateType.Move);
        }
        Vector2 nowPosition = enemy.transform.position;
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            SeManager.Instance.StopImmediately("Drone");
            SeManager.Instance.Play("Drone");
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
            enemyRigidBody2D.linearVelocity = Vector2.zero;
            return;
        }
        // Rigidbody2Dで移動
        enemyRigidBody2D.linearVelocity = directionToNext.normalized * enemy.moveSpeed * GameTimer.Instance.customTimeScale / 2;

        playerCheack.RotationFoward(enemy.transform);
        // 発射レートを設定しその後、発射秒数を決定する。
        switch (shotSection)
        {
            case ShotSection.aim:

                enemyRigidBody2D.linearVelocity = Vector2.zero;
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (enemy.aimTime <= timer)
                {
                    shotSection++;
                    timer = 0;
                }
                break;
            case ShotSection.shot:
                if (enemy.shotIntervalTime >= 100)
                {
                    shotSection++;
                }
                else
                {
                    // 拡散率加算
                    enemy.diffusionRate += enemy.recoil;
                    // 拡散率を固定、下限enemy.minDiffusionRate、上限 enemy.maxDiffusionRate
                    Mathf.Clamp(enemy.diffusionRate, enemy.minDiffusionRate, enemy.maxDiffusionRate);
                    // 射撃
                    bulletGeneratar.GunFire(enemy.bulletSpeed, enemy.hitDamegeLayer, enemy.stoppingPower, enemy.diffusionRate);

                    enemy.NOWBULLET--;
                    shotSection++;
                }
                break;
            case ShotSection.shotInterval:
                timer += GameTimer.Instance.ScaledDeltaTime;
                if (enemy.shotIntervalTime <= timer)
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