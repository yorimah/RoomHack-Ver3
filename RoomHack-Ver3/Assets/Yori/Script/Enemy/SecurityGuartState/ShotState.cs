using UnityEngine;

public class ShotState : IState
{
    private SecurityGuard _securityGuard;
    public ShotState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }
    public void Enter()
    {
        Debug.Log("ShotState: Enter");
    }

    public void Execute()
    {
        Debug.Log("ShotState: Execute");
    }

    public void Exit()
    {
        Debug.Log("ShotState: Exit");
    }
}