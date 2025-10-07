using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FlySecurityDrone : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new DroneMoveState(this) },
        { StateType.Shot, new DroneShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Bind, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }
}
