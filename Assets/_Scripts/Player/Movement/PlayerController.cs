using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] float Speed = 3.0f;
    [SerializeField] float multiplier = 2.0f;

    [SerializeField] bool sprint = false;

    [Header("Camera Settings")]
    [SerializeField] bool invertYAxis = false;

    [Header("Look Sensitivity")]
    [SerializeField] float mouseSensitivity = 2.0f;
    [SerializeField] float upDownEange = 80.0f;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHadler inputHadler;
    private Vector3 currentMovement;
    private float verticalRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputHadler = PlayerInputHadler.Instance;
    }

    private void Update()
    {
        HadlerMovement();
        HadlerRotation();
    }

    void HadlerMovement()
    {
        float speed = Speed * (sprint ? multiplier : 1f);

        Vector3 inputDirection = new(inputHadler.MoveInput.x, 0f, inputHadler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        currentMovement.x = worldDirection.x * speed;
        currentMovement.y = worldDirection.y * speed;
        currentMovement.z = worldDirection.z * speed;

        characterController.Move(currentMovement * Time.deltaTime);
    }

    void HadlerRotation()
    {
        float mouseYInput = invertYAxis ? -inputHadler.LookInput.y : inputHadler.LookInput.y;

        float mouseXRotation = inputHadler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= mouseYInput * mouseSensitivity;
        //verticalRotation -= inputHadler.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownEange, upDownEange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
