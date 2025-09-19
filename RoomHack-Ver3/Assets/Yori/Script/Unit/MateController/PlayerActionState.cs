using UnityEngine;

public class PlayerActionState : IState
{
    private UnitCore unitCore;
    private PlayerMove playerMove;
    private PlayerShot playerShot;
    public PlayerActionState(UnitCore _unitCore)
    {
        unitCore = _unitCore;
        playerMove = new PlayerMove(unitCore);
        playerShot = new PlayerShot(unitCore);
    }
    public void Enter()
    {

    }
    public void Execute()
    {
        playerMove.PlMove();
        playerShot.Shot();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            unitCore.ChangeState(UnitCore.StateType.Hack);
        }
    }

    public void Exit()
    {

    }    
}
