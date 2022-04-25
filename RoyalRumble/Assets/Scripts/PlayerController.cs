using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Player float values.
    [SerializeField]
    public float playerSpeed = 2.0f;

    [SerializeField]
    private float gravityValue = -9.81f;

    public float smoothInputSpeed = 0.2f;

    [SerializeField]
    private float controllerDeadzone = 0.1f;

    [SerializeField]
    private float gamepadRotateSmoothing = 1000f;


    // Player int values.
    public int playerID;


    // Player controller / input references.
    private CharacterController controller;
    //private PlayerControls playerControls;
    //public PlayerInput playerInput;


    // Player Vector3 variables.
    private Vector3 playerVelocity;
    public Vector3 startPos;
    private Vector3 move;


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
    private bool hasDevice = false;
    private bool isGamepad;


    // Called once at the beginning of the game.
    private void Start()
    {
        // Reference player controls.
        controller = gameObject.GetComponent<CharacterController>();

        // Places player as spawn location.
        transform.position = startPos;
    }
    private void Awake()
    {
        // Reference player input and determine control scheme.
        //playerInput = gameObject.GetComponent<PlayerInput>();
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
        // Gives the ability to freeze players if needed.
        if (canMove)
        {
            HandleMovement();
        }

        if (canControl)
        {
            if (isGamepad)
            {
                HandleGamepadRotation();
            }
            else
            {
                HandleMouseRotation();
            }
        }
    }

    private void HandleMovement()
    {
        // Makes sure player is level with ground.
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

    // Takes input from keyboard & mouse wielding players.
    public void getMouseRotVector(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aim = context.ReadValue<Vector2>();

            if (hasDevice == false)
            {
                isGamepad = false;
                hasDevice = true;
            }
        }

    }

    // Takes input from gamepad wielding players.
    public void getStickRotVector(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            aim = context.ReadValue<Vector2>();
        }
        else if (context.performed)
        {
            aim = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {

            aim = context.ReadValue<Vector2>();
        }

        if (hasDevice == false)
        {
            isGamepad = true;
            hasDevice = true;
        }
    }

    // Rotates keyboard & mouse wielding players.
    public void HandleMouseRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(aim);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            LookAt(point);
        }
    }

    // Rotates gamepad wielding players.
    private void HandleGamepadRotation()
    {
        if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, gamepadRotateSmoothing * Time.deltaTime);
            }
        }
    }

    // Corrects player look direction for mouse so that player doesn't look above an object if the mouse is not pointing at a flat object.
    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
