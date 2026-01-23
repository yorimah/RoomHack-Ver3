using UnityEngine;
using Cysharp.Threading.Tasks;
public class TurretGunMoveState : IEnemyState
{
    private EnemyBase enemy;

    private float flipTimer = 0;

    private float flipInterval = 2;

    private int direction = 1;

    float rotation;
    private PlayerCheck playerCheck;
    public TurretGunMoveState(EnemyBase _enemy)
    {
        enemy = _enemy;
        playerCheck = enemy.playerCheck;
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        if (playerCheck.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask(), enemy.PlayerPosition))
        {
            enemy.ChangeState(EnemyStateType.Shot);
        }
        else
        {
            if (flipTimer >= flipInterval)
            {
                SeManager.Instance.Play("TurretMove");
                direction = Random.value < 0.5f ? -1 : 1;
                flipTimer = 0f;
            }
            else
            {
                rotation += 10 * direction * GameTimer.Instance.GetScaledDeltaTime();
                enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                flipTimer += GameTimer.Instance.GetScaledDeltaTime();
            }
        }
    }

    public void Exit()
    {

    }
}