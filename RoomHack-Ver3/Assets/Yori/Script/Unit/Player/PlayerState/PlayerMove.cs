using UnityEngine;

public class PlayerMove
{
    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private Vector3 mousePosition;

    private Vector3 direction;

    private float moveSpeed;

    IPlayerInput playerInput;

    public PlayerMove(Rigidbody2D _playerRigidBody, float _moveSpeed, IPlayerInput _playerInput)
    {
        moveSpeed = _moveSpeed;
        playerRigidbody2D = _playerRigidBody;
        playerInput = _playerInput;
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

    public void playerMove()
    {
        PlayerRotation();
        playerRigidbody2D.linearVelocity = PlayerMoveVector(playerInput.MoveValue(), moveSpeed);
    }

    public void Blink()
    {
        float blinkNum = 3;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerRigidbody2D.transform.position;
        pos.z = 0;
        playerRigidbody2D.transform.position += pos.normalized * blinkNum;
    }

    public void EdgeRun()
    {
        PlayerRotation();
        playerRigidbody2D.linearVelocity = PlayerMoveVector(playerInput.MoveValue(), moveSpeed);
    }
}
