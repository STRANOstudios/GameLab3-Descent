using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] float Speed = 3.0f;
    [SerializeField] float multiplier = 2.0f;
    [SerializeField] float flySpeed = 1.0f;

    [SerializeField] float smoothFactor = 0.3f;
    [SerializeField] bool sprint = false;

    [SerializeField] float floatingOscillationAmount = 0.5f;
    [SerializeField] float floatingOscillationSpeed = 1f;
    [Space]
    [SerializeField] float bankingSpeed = 1.0f;
    [SerializeField] float smoothness = 0.1f;

    [Header("Camera Settings")]
    [SerializeField] bool invertYAxis = false;
    [SerializeField] float oscillationAmount = 1f;
    [SerializeField] float oscillationSpeed = 1f;
    [SerializeField] float acceleration = 1f;

    [Header("Look Sensitivity")]
    [SerializeField] float mouseSensitivity = 2.0f;
    [SerializeField] float upDownEange = 80.0f;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHadler inputHandler;
    private Vector3 currentMovement;
    private float verticalRotation;
    public bool IsBankable = false;

    private float currentOscillation = 0f;
    private float currentOscillationVelocity = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputHandler = PlayerInputHadler.Instance;
    }

    private void Update()
    {
        HandlerMovement();
        HandlerRotation();
        //HandlerBanking();

        ApplyFloatingOscillation();
    }

    private void HandlerBanking()
    {
        if (inputHandler.bankIsActiveTrigger)
        {
            IsBankable = !IsBankable;
        }
        if (!IsBankable) return;

        float targetBankAngle = -inputHandler.bankValue * 1f * bankingSpeed;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetBankAngle);

        //continua a bloccare la rotazione sull'asse delle y
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);

        //devo creare il blocco sulla rotazione
    }

    void HandlerMovement()
    {
        float speedMultiplier = sprint ? multiplier : 1f;
        float speed = Speed * speedMultiplier;

        Vector2 moveInput = inputHandler.MoveInput;
        Vector3 inputDirection = new Vector3(moveInput.x, inputHandler.FlyValue, moveInput.y).normalized;

        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        Vector3 targetMovement = worldDirection * speed;

        currentMovement = Vector3.Lerp(currentMovement, targetMovement, smoothFactor * Time.deltaTime);
        characterController.Move(currentMovement * Time.deltaTime);
    }

    void HandlerRotation()
    {
        float mouseYInput = invertYAxis ? -inputHandler.LookInput.y : inputHandler.LookInput.y;

        float mouseXRotation = inputHandler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);


        //view vfx
        verticalRotation -= mouseYInput * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownEange, upDownEange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        float horizontalRotation = Mathf.Abs(inputHandler.LookInput.x);
        float targetOscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmount * horizontalRotation;

        currentOscillation = Mathf.SmoothDamp(currentOscillation, targetOscillation, ref currentOscillationVelocity, acceleration);

        mainCamera.transform.Rotate(0, 0, currentOscillation);
    }

    void ApplyFloatingOscillation()
    {
        //vfx
        Transform mainCameraTransform = mainCamera.transform;
        float floatingOscillation = Mathf.Sin(Time.time * floatingOscillationSpeed) * floatingOscillationAmount;
        mainCameraTransform.localPosition = new Vector3(mainCameraTransform.localPosition.x, floatingOscillation, mainCameraTransform.localPosition.z);
    }
}
