using UnityEngine;
using UnityEngine.InputSystem;

public class Pupil : MonoBehaviour
{
    private RectTransform pupilTransform;
    [SerializeField] private float rotationSpeed;

    private void Awake()
    {
        pupilTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector2 direction = new Vector2(pupilTransform.position.x - mousePosition.x, pupilTransform.position.y - mousePosition.y);

        float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;

        Quaternion target = Quaternion.Euler(0f, 0f, angle);
        pupilTransform.rotation = Quaternion.Slerp(pupilTransform.rotation, target, rotationSpeed * Time.deltaTime);
    }


}
