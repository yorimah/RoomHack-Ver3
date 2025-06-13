using UnityEngine;

public class IdleState : IState
{
    private SecurityGuard _securityGuard;

    public IdleState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
    }

    public void Enter()
    {
        Debug.Log("IdleState: Enter");
    }

    public void Execute()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(_securityGuard.transform.position);

        // 画面内判定　入ったらtrue
        bool isInside =
            viewportPos.x >= 0 && viewportPos.x <= 1 &&
            viewportPos.y >= 0 && viewportPos.y <= 1;
        if (isInside)
        {
            _securityGuard.ChangeState(new MoveState(_securityGuard));
        }
    }

    public void Exit()
    {
        Debug.Log("IdleState: Exit");
    }
}

