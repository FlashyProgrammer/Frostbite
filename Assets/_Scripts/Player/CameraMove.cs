using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Sprite gameCursor;
    
    [Header("Mouse Input")]
    [SerializeField] private float mouseSensitivityX = 50f;
    [SerializeField] private float mouseSensitivityY = 100f;

    [Header("Controller Input")]
    [SerializeField] private float controllerSensitivityX = 50f;
    [SerializeField] private float controllerSensitivityY = 100f;

    private float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {

        if (Mouse.current != null) 
        {
            float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivityY * Time.deltaTime;
            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivityX * Time.deltaTime;
            xRotation -= mouseY;
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            player.Rotate(Vector3.up * mouseX);

        }

        if(Gamepad.current != null)
        {
            float stickY = Gamepad.current.rightStick.y.ReadValue() * controllerSensitivityY * Time.deltaTime;
            float stickX = Gamepad.current.rightStick.x.ReadValue() * controllerSensitivityX * Time.deltaTime;
            xRotation -= stickY;
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            player.Rotate(Vector3.up * stickX);
        }

    }
}
