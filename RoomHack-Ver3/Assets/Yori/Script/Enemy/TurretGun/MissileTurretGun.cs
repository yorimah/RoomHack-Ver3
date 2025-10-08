using System.Collections.Generic;

public class MissileTurretGun : Enemy
{
    public void Start()
    {
        moveSpeed = 0;
        playerCheack = new PlayerCheack();

        states = new Dictionary<StateType, IState>()
    {
        { StateType.Idle, new IdleState(this) },
        { StateType.Move, new TurretGunMoveState(this) },
        { StateType.Shot, new MissileShotState(this) },
        { StateType.Die, new DieState(this) },
    };
        statetype = StateType.Idle;
        currentState = states[statetype];

        canHackToolTag = new List<toolTag> { toolTag.CCTVHack, toolTag.Blind, toolTag.OverHeat, toolTag.Detonation };
    }
}
