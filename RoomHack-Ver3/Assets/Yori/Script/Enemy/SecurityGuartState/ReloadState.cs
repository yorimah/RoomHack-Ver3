using UnityEngine;

public class ReloadState : IState
{
    private SecurityGuard _securityGuard;
    public ReloadState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }

    private float timer = 0;
    public void Enter()
    {
        timer = 0;
    }

    public void Execute()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            _securityGuard.ChangeState(SecurityGuard.StateType.Shot);
            timer = 0;
        }
    }

    public void Exit()
    {
        _securityGuard.nowMagazine = _securityGuard.gundata.MaxMagazine;
    }
}