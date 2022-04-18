using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Player float values.
    [SerializeField]
    public float playerSpeed = 2.0f;  //Made this public so it can be accessed by the Sand Hazard

    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float smoothInputSpeed = 0.2f;

    [SerializeField]
    private float controllerDeadzone = 0.1f;

    [SerializeField]
    private float gamepadRotateSmoothing = 1000f;


    // Player int values.
    public int playerID;


    // Player controller / input references.
    private CharacterController controller;
    private PlayerControls playerControls;
    public PlayerInput playerInput;


    // Player Vector3 variables.
    private Vector3 playerVelocity;
    public Vector3 startPos;
    private Vector3 move;
    public Vector3 mouseAim;


    // Player Vector2 variables.
    private Vector2 movementInput = Vector2.zero;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    public Vector2 aim;


    // Player boolean variables.
    private bool groundedPlayer;
    public bool isBot;
    public bool canMove = false;
    public bool canControl = false;

    [SerializeField]
    private bool isGamepad;


    // Called once at the beginning of the game.
    private void Start()
    {
        // Reference player controls.
        controller = gameObject.GetComponent<CharacterController>();
        //playerControls = new PlayerControls();

        // Enable player controls / input.

        //playerControls.Enable();
        //playerControls.Player.Enable();

        // Places player as spawn location.
        transform.position = startPos;
    }
    private void Awake()
    {
        // Reference player input and determine control scheme.
        playerInput = gameObject.GetComponent<PlayerInput>();


        isGamepad = playerInput.currentControlScheme.Equals("Gamepad");
    }

    // This function allows movement input to be accessible via 
    // keyboard & controller.
    public void OnMove(InputAction.CallbackContext context)
    {
        // Give ability to take control from player and zero-out movement input if needed.
        if (canControl)
        {
            // Reads movement input based on input device.
            movementInput = context.ReadValue<Vector2>();
        }
        else
            movementInput = new Vector2(0, 0);
    }

    // Called once between frames.
    private void FixedUpdate()
    {
        InputSystem.Update();
        //HandleRotationInput();
        // Gives the ability to freeze players if needed.
        if (canMove)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        // This code makes sure the player stays level to the ground
        // as long as they are ground. Though this code is not too 
        // useful currently, it could be used later for knockback effect(s).
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Receives movement input and smoothly applies it to player.
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movementInput, ref smoothInputVelocity, smoothInputSpeed);
        move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Applies gravity to player.
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(new Vector2(0, playerVelocity.y) * Time.deltaTime);
    }

    // Takes input to control where the player will rotate depending on control scheme.
    private void HandleRotationInput()
    {
        aim = playerControls.Player.Aim.ReadValue<Vector2>();
        mouseAim = playerControls.Player.MouseAim.ReadValue<Vector2>();
    }
    public void getMouseRotVector(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mouseAim = context.ReadValue<Vector3>();
            //Debug.Log(mouseAim);
        }

    }
    public void getStickRotVector(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aim = context.ReadValue<Vector2>();
            //Debug.Log(aim);
        }
    }

    // Rotates player depending on control scheme.
    private void HandleRotation()
    {
        // If player is using controller, rotate with the right stick.
        if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, gamepadRotateSmoothing * Time.deltaTime);
            }
        }
        // If the player is using keyboard & mouse, rotate using the mouse.
        /*else
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseAim);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }*/
    }

    // Corrects player look direction for mouse so that player doesn't look above an object if the mouse is not pointing at a flat object.
    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    // Functions to ensure players are using the correct aiming mechanic based on their control scheme.
    public void isMouseAim()
    {
        isGamepad = false;
    }
    public void isGamepadAim()
    {
        isGamepad = true;
    }


}
