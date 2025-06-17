using UnityEngine;

public class ReloadState : IState
{
    private SecurityGuard _securityGuard;
    public ReloadState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }
    public void Enter()
    {
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}