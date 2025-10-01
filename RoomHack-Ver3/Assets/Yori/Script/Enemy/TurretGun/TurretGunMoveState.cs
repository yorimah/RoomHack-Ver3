using UnityEngine;

public class TurretGunMoveState : IState
{
    private Enemy enemy;

    private float flipTimer = 0;

    private float flipInterval = 1;

    private int direction = 1;

    float rotation;
    private PlayerCheack playerCheack;
    public TurretGunMoveState(Enemy _enemy)
    {
        enemy = _enemy;
        playerCheack = enemy.playerCheack;
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        if (playerCheack.PlayerRayHitCheack(enemy.transform, enemy.GetObstacleMask()))
        {
            enemy.ChangeState(Enemy.StateType.Shot);
        }
        else
        {
            if (flipTimer >= flipInterval)
            {
                direction = Random.value < 0.5f ? -1 : 1;
                flipTimer = 0f;
            }
            else
            {
                rotation += enemy.moveSpeed * direction * GameTimer.Instance.ScaledDeltaTime;
                enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                flipTimer += GameTimer.Instance.ScaledDeltaTime;
            }
        }
    }

    public void Exit()
    {

    }
}