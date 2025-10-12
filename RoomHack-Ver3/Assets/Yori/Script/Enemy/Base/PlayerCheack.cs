using UnityEngine;

public class PlayerCheack
{
    /// <Summary>
    /// レイを飛ばして壁にあったたらfalseあたらなかったらtrue
    /// </Summary>
    public bool PlayerRayHitCheack(Transform transform, LayerMask obstacleMask)
    {
        if (PlayerInsNullCheack())
        {
            return false;
        }
        else
        {
            Vector2 playerPosition = UnitCore.Instance.transform.position;
            float playerDistance = Vector2.Distance(transform.position, playerPosition);
            Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, playerDistance, obstacleMask);

            Debug.DrawRay(transform.position, playerDirection * playerDistance, Color.red);

            if (hit.collider != null)
            {
                return false;
            }
        }

        return true;
    }

    /// <Summary>
    /// プレイヤーの方向を向くよ。引数は自分のポジションを入れてね
    /// </Summary>
    public void RotationFoward(Transform transform)
    {
        if (PlayerInsNullCheack())
        {
            return;
        }
        Vector2 playerPosition = UnitCore.Instance.transform.position;
        Vector2 playerNextPosition = playerPosition + UnitCore.Instance.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        Vector2 direction = playerNextPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }

    private bool PlayerInsNullCheack()
    {
        if (UnitCore.Instance == null)
        {
            return true;
        }
        return false;
    }
}
