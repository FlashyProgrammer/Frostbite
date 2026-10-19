using UnityEngine;
using UnityEngine.InputSystem;


public class Marker : MonoBehaviour
{
    [HideInInspector] public bool canCheck;
    [SerializeField] private Minigame minigame;

    [Header("Marker Parameters")]
    [SerializeField] private bool isAlive;
    [SerializeField] private float moveSpeed;
    [SerializeField] private RectTransform markerPosition;

    private RectTransform markerTransform;
    private Vector2 inputValue;
    private PlayerControls moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        moveInput = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        markerTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        moveInput.Enable();
    }
    private void OnDisable()
    {
        moveInput.Disable();
    }
    private int areaCount;

    private void FixedUpdate()
    {
        if (isAlive)
        {
            rb.linearVelocity = inputValue * moveSpeed;
        }
        ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlive)
        {
            if (collision.CompareTag("Area")) areaCount++;
        }

        else
        {
            if (collision.CompareTag("Area"))
            {
                if (markerPosition != null)
                {
                    markerTransform.position = markerPosition.position;
                    inputValue = Vector2.zero;
                }

                minigame.Completed();

            }


            if (collision.CompareTag("Obstacle")) 
            {
                if (markerPosition!=null)
                {
                    markerTransform.position = markerPosition.position;
                    inputValue = Vector2.zero;
                }

                minigame.Reset();
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAlive)
        {
            if (collision.CompareTag("Area")) areaCount--;
        }

      
    }

    public void Check()
    {
        if (!canCheck) 
        {
            return;
        }

        else
        {
            if (areaCount > 0)
            {
                minigame.Completed();
            }


            else
            {
                minigame.Reset();
                canCheck = false;
            }
             
        }

    }

    public void MoveUI(InputAction.CallbackContext context)
    {
        if (isAlive && this.isActiveAndEnabled)
        {
            inputValue = context.ReadValue<Vector2>().normalized;

        }
    }
}
