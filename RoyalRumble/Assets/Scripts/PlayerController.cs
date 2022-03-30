using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Script references.
    public gameManager manager;


    // Player float values.
    [SerializeField]
    private float playerSpeed = 2.0f;

    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float smoothInputSpeed = 0.2f;


    // Player int values.
    public int playerID;


    // Player controller reference.
    private CharacterController controller;


    // Player Vector3 variables.
    private Vector3 playerVelocity;
    public Vector3 startPos;
    private Vector3 move;


    // Player Vector2 variables.
    private Vector2 movementInput = Vector2.zero;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;


    // Player boolean variables.
    private bool groundedPlayer;
    public bool canMove = false;

    // Called once at the beginning of the game.
    private void Start()
    {
        // Finds References.
        controller = gameObject.GetComponent<CharacterController>();
        manager = FindObjectOfType<gameManager>();

        // Places player as spawn location.
        transform.position = startPos;
    }

    // This function allows movement input to be accessible via 
    // keyboard & controller.
    public void OnMove(InputAction.CallbackContext context)
    {
        // Reads movement input based on input device.
        movementInput = context.ReadValue<Vector2>();
    }

    // Called once per frame.
    private void Update()
    {

        // Gives the ability to freeze players if needed.
        if (canMove)
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

            // Stops player when there is no input.
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Applies gravity to player.
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    void OnCollision(Collision collision)
    {   
        //Checks for collision with pit hazard, kills player if so
        if(collision.collider.tag == "deathHole")
        {
            GameObject gameManager = GameObject.Find("Game Manager");
            hazardManager hManager = gameManager.GetComponent<hazardManager>();
            hManager.holeDeath = false;
        }
    }
}
