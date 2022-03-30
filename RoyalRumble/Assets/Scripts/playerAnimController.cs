using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerAnimController : MonoBehaviour
{
    public Animator anim;
    public CharacterController characterController;
    public combatController combat;
    public PlayerInput input;
    public float refVel;
    public bool isMoving;
    public Vector2 moveInput;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {

    }
    public void moveAnim(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.magnitude > .1f)
        {
            anim.SetBool("isMoving", true);
            isMoving = true;
        }
        if (moveInput.magnitude == 0f)
        {
            anim.SetBool("isMoving", false);
            isMoving = false;
        }
    }
    public void tossAnim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.Play("Throw");
        }
    }
    public void punchAnim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.Play("Punch");
        }
    }
}