using System.Collections.Generic;
public class TurretGun : Enemy
{
    public void Start()
    {
        moveSpeed = 0;
        playerCheck = new PlayerCheck();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new TurretGunMoveState(this) },
        { EnemyStateType.Shot, new TurretGunShotState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        cantHackToolType = new List<ToolType> { };
    }
}