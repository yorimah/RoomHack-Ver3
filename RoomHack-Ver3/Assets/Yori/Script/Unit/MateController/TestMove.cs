using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove :MonoBehaviour
{
    private MoveInput moveInput ;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private float MOVESPEED = 10;
    public void Start()
    {
        moveInput = new MoveInput();

        moveInput.Init();

        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue()) * MOVESPEED;
    }

    public Vector2 PlayerMoveVector(Vector2 inputMoveVector)
    {
        moveVector = inputMoveVector;
        return moveVector;
    }
}
