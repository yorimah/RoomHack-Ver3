using System.Collections.Generic;
public class TurretGun : Enemy
{
    public void Start()
    {
        moveSpeed = 0;
        playerCheack = new PlayerCheack();

        states = new Dictionary<EnemyStateType, IState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new TurretGunMoveState(this) },
        { EnemyStateType.Shot, new TurretGunShotState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }
}