using UnityEngine;

public class DieState : IState
{
    private SecurityGuard _securityGuard;
    public DieState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }
    public void Enter()
    {
        _securityGuard.gameObject.SetActive(false);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
    }
}