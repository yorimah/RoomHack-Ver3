using UnityEngine;

public class PlayerMove
{
    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private IGetMoveSpeed getMoveSpeed;

    IPlayerInput playerInput;

    public PlayerMove(Rigidbody2D _playerRigidBody, IGetMoveSpeed _getMoveSpeed, IPlayerInput _playerInput)
    {
        getMoveSpeed = _getMoveSpeed;
        playerRigidbody2D = _playerRigidBody;
        playerInput = _playerInput;
    }

    public Vector2 PlayerMoveVector(Vector2 inputMoveVector, float moveSpeed)
    {
        moveVector = inputMoveVector * moveSpeed;
        return moveVector;
    }

    public void playerMove()
    {
        if (playerRigidbody2D != null)
        {

            playerRigidbody2D.linearVelocity = PlayerMoveVector(playerInput.MoveValue(), getMoveSpeed.MoveSpeed);
        }
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
        playerRigidbody2D.linearVelocity = PlayerMoveVector(playerInput.MoveValue(), getMoveSpeed.MoveSpeed);
    }
}
