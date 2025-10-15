using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class SecurityGuard : Enemy
{
    void Start()
    {
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new MoveState(this) },
        { StateType.Shot, new ShotState(this) },
        { StateType.Reload, new ReloadState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Bind, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
        nowHackEvent = new List<ToolEvent>();
    }
}
