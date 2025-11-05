using System.Collections.Generic;
using Zenject;
public class MissileTurretGun : Enemy
{
    [Inject]
    Missile.Factory missileFactory;
    public void Start()
    {
        moveSpeed = 0;
        playerCheack = new PlayerCheack();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new TurretGunMoveState(this) },
        { EnemyStateType.Shot, new MissileShotState(this,missileFactory) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }
}
