using UnityEngine;

public class PlayerMove
{
    private Player unitCore;

    private PlayerInput moveInput;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private Vector3 mousePosition;

    private Vector3 direction;

    public PlayerMove(Player _unitCore)
    {
        unitCore = _unitCore;
        playerRigidbody2D = unitCore.GetComponent<Rigidbody2D>();
        moveInput = unitCore.playerInput;
    }
    public Vector2 PlayerMoveVector(Vector2 inputMoveVector, float moveSpeed)
    {
        moveVector = inputMoveVector * moveSpeed;
        return moveVector;
    }

    private void PlayerRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - unitCore.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        unitCore.transform.rotation = targetRotation;
    }

    public void PlMove()
    {
        PlayerRotation();
        playerRigidbody2D.linearVelocity = PlayerMoveVector(moveInput.MoveValue(), unitCore.moveSpeed) * GameTimer.Instance.customTimeScale;
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
        playerRigidbody2D.linearVelocity = PlayerMoveVector(moveInput.MoveValue(), unitCore.moveSpeed);
    }
}
