using System.Collections.Generic;
using Zenject;
public class MissileTurretGun : Enemy
{
    [Inject]
    Missile.Factory missileFactory;
    public void Start()
    {
        moveSpeed = 0;
        playerCheck = new PlayerCheck();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new TurretGunMoveState(this) },
        { EnemyStateType.Shot, new MissileShotState(this,missileFactory) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<ToolTag> { ToolTag.VisionHack, ToolTag.WeponGlitch, ToolTag.OverHeat, ToolTag.Detonation, ToolTag.BitClack };
    }
}
