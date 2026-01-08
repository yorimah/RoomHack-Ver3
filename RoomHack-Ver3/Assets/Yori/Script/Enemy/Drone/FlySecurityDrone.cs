using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FlySecurityDrone : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<EnemyStateType, IEnemyState>()
    {
        { EnemyStateType.Idle, new IdleState(this) },
        { EnemyStateType.Move, new DroneMoveState(this) },
        { EnemyStateType.Shot, new DroneShotState(this) },
        { EnemyStateType.Reload, new ReloadState(this) },
        { EnemyStateType.Die, new DieState(this) },
    };
        statetype = EnemyStateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<ToolTag> { ToolTag.VisionHack, ToolTag.DeadRock, ToolTag.WeponGlitch, ToolTag.OverHeat, ToolTag.Detonation, ToolTag.BitClack, ToolTag.Disruption};
        nowHackEvent = new List<ToolEventBase>();
    }
}
