using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    playerInputManager inputManager;
    Rigidbody rb;
    [SerializeField] float speed;
    public Vector2 moveInput;
    void Start()
    {
        inputManager = GetComponent<playerInputManager>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {

    }
    private void FixedUpdate()
    {
        move();
    }
    public void move() // Debug purposes.
    {
        Vector3 moveDir = new Vector3(moveInput.x * speed * Time.fixedDeltaTime, 0, moveInput.y * speed * Time.fixedDeltaTime);
        if (moveInput == Vector2.zero)
            return;
        rb.MovePosition(transform.position + moveDir);
    }
}
