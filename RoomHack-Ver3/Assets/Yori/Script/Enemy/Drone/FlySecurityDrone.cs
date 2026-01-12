using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FlySecurityDrone : Enemy
{
    void Start()
    {
        playerCheck = new PlayerCheck();

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

        cantHackToolType = new List<ToolType> { };
        nowHackEvent = new List<ToolEventBase>();

    }
}
