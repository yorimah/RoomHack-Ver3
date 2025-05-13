using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private MoveInput moveInput;

    private Vector2 moveVector;

    private Rigidbody2D playerRigidbody2D;

    private float MOVESPEED = 10;

    private Vector3 mousePosition;
    public void Start()
    {
        moveInput = new MoveInput();

        moveInput.Init();

        playerRigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        playerRigidbody2D.velocity = PlayerMoveVector(moveInput.MoveValue(), MOVESPEED);
        PlayerRotation();
    }

    public Vector2 PlayerMoveVector(Vector2 inputMoveVector, float moveSpeed)
    {
        moveVector = inputMoveVector * moveSpeed;
        return moveVector;
    }

    private void PlayerRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - this.transform.position;

        // ベクトルを角度に変換して敵を向く
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 5f);
    }

    [SerializeField,Header("弾のプレハブ")]
    private GameObject bulletPrefab;
    [SerializeField, Header("弾のスピード")]
    private float bulletSpeed;

    private void Shot()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rigidBullet = bulletGameObject.GetComponent<Rigidbody2D>();


        
    }
}
