using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMove
{
    private UnitCore unitCore;

    private MoveInput moveInput;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private Vector3 mousePosition;

    private Vector3 direction;

    
    public PlayerMove(UnitCore _unitCore)
    {
        unitCore = _unitCore;
        playerRigidbody2D = unitCore.GetComponent<Rigidbody2D>();
        moveInput = unitCore.moveInput;
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
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), unitCore.moveSpeed) * GameTimer.Instance.customTimeScale;
    }
}
