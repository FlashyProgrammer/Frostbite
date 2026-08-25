using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls inputs;
    private CharacterController player;

    private bool isMoving;
    private bool isSprinting;
    private bool isCrouching;
    private int crouchCounter;
    private float playerHeight;
    private Vector3 playerDirection;
    private Vector3 playerVelocity;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float crouchSpeed = 3.0f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float playerFriction = 2.0f;
    [SerializeField] private float playerGravity = -9.8f;

    private void Awake()
    {
        inputs = new PlayerControls();
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        playerHeight = player.height;

    }

    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        if (isMoving)
        {
            Movement();
            
        }
        else
        {
            playerVelocity = Vector3.Lerp(playerVelocity, Vector3.zero, playerFriction * Time.deltaTime);
        }

        playerVelocity.y += playerGravity * Time.deltaTime;
        player.Move(playerVelocity);
         
    }

    private void Movement()
    {
        playerDirection = transform.forward * inputs.Player.Move.ReadValue<Vector3>().z + transform.right * inputs.Player.Move.ReadValue<Vector3>().x;

        // Walking
        if (player.isGrounded && !isSprinting)
        {
            playerVelocity = playerDirection * moveSpeed * Time.deltaTime;
         
        }
        // Sprinting
        if (player.isGrounded && isSprinting)
        {
            playerVelocity = playerDirection * sprintSpeed * Time.deltaTime;

        }
        // Crouching
        if(player.isGrounded && isCrouching)
        {
            playerVelocity = playerDirection * crouchSpeed * Time.deltaTime;
        }
    }

    public void CanMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMoving = true;
          
        }

        if (context.canceled)
        {
            isMoving = false;

        }

    }
    public void CanSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }

        if (context.canceled)
        {
            isSprinting = false;

        }

    }

    public void CanCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && crouchCounter == 0)
        {
            isCrouching = true;
            player.height = crouchHeight;
            crouchCounter++;

        }

        if (context.canceled && crouchCounter == 1)
        {
            crouchCounter++;
        }

        if(context.performed && crouchCounter == 2)
        {
            isCrouching = false;
            isMoving = true; player.height = playerHeight;
            crouchCounter = 0;
        }
    }
}
