using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class playerInputManager : MonoBehaviour
{
    PlayerInput input;
    movePlayer movePlayer;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        movePlayer = GetComponent<movePlayer>();
    }
    void Start()
    {

    }
    void Update()
    {
    }
    private void FixedUpdate()
    {

    }
    public void attack(InputAction.CallbackContext context)
    {

    }
    public void toss(InputAction.CallbackContext context)
    {

    }
    public void onMove(InputAction.CallbackContext context)
    {
        movePlayer.moveInput = context.ReadValue<Vector2>();
    }
}
