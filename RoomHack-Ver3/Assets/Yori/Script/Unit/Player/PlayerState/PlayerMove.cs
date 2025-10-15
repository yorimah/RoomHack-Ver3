using UnityEngine;

public class PlayerMove
{
    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private Vector3 mousePosition;

    private Vector3 direction;

    private float moveSpeed;

    public PlayerMove(Rigidbody2D _playerRigidBody, float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
        playerRigidbody2D = _playerRigidBody;
    }

    public Vector2 PlayerMoveVector(Vector2 inputMoveVector, float moveSpeed)
    {
        moveVector = inputMoveVector * moveSpeed;
        return moveVector;
    }

    private void PlayerRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - playerRigidbody2D.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        playerRigidbody2D.transform.rotation = targetRotation;
    }

    public void playerMove(Vector2 inputMoveVector)
    {
        PlayerRotation();
        playerRigidbody2D.linearVelocity = PlayerMoveVector(inputMoveVector, moveSpeed);
    }
}
