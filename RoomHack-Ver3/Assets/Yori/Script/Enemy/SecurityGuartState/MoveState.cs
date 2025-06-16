using UnityEngine;

public class MoveState : IState
{
    private SecurityGuard _securityGuard;

    private Rigidbody2D secRididBody;

    private float flipTimer = 0;

    private float flipInterval = 1;

    private int direction = 1;

    public MoveState(SecurityGuard securityGuard)
    {
        _securityGuard = securityGuard;
        secRididBody = _securityGuard.GetComponent<Rigidbody2D>();
    }

    public void Enter()
    {
      
        flipTimer = 0;

        flipInterval = 1;

        direction = 1;
    }

    public void Execute()
    {
        Vector2 nowPosition = _securityGuard.transform.position;
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            direction = Random.value < 0.5f ? -1 : 1;
            flipTimer = 0f;
        }

        Vector2 center = UnitCore.Instance.transform.position;
        Vector2 dir = nowPosition - center;

        Vector2 emDir = new Vector2(-dir.y, dir.x);
        Vector2 nextPos = (nowPosition + (emDir * direction));

        // 障害物チェック
        Vector2 directionToNext = (nextPos - nowPosition).normalized;
        float checkDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(nowPosition, directionToNext, checkDistance,_securityGuard.GetObstacleMask());
        //Debug.DrawRay(nowPosition, directionToNext * checkDistance, Color.blue);
        if (hit.collider != null)
        {
            direction *= -1;
            flipTimer = 0f;
            secRididBody.velocity = Vector2.zero;
            return;
        }
        // Rigidbody2D で移動
        secRididBody.velocity = directionToNext.normalized * 5;
    }

    public void Exit()
    {
       
    }
}

