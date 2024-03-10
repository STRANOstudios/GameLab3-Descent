using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] float Speed = 3.0f;
    [SerializeField] float sprintMultiplier = 2.0f;
    [SerializeField] float movementSmoothFactor = 0.3f;
    [SerializeField] bool sprint = false;

    [Header("Camera Settings")]
    [SerializeField] float acceleration = 1f;
    [SerializeField] float viewSmoothFactor = 5f;
    [SerializeField] bool invertYAxis = false;

    [Header("Look Sensitivity")]
    [SerializeField] float mouseSensitivity = 2.0f;
    [SerializeField] float upDownEange = 80.0f;

    [Header("Rotation Settings")]
    [SerializeField] float rotationSmoothFactor = 0.1f;

    [Header("VFX")]
    [SerializeField] float oscillationAmount = 1f;
    [SerializeField] float oscillationSpeed = 1f;
    [SerializeField] float floatingOscillationAmount = 0.5f;
    [SerializeField] float floatingOscillationSpeed = 1f;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHadler inputHandler;
    private Vector3 currentMovement;
    private float verticalRotation;
    private bool IsBankable = false;
    private float currentOscillation = 0f;
    private float currentOscillationVelocity = 0f;
    private bool bankIsActive = true;
    private float originalRotationZ = 0f;
    private float rotationAmount = -300f;

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
        HandlerBanking();

        ApplyFloatingOscillation();
    }

    private void HandlerBanking()
    {
        if (inputHandler.bankIsActiveTrigger && bankIsActive)
        {
            IsBankable = !IsBankable;
            StartCoroutine(DelayedBankingToggle(0.1f));
        }
        if (!IsBankable) return;

        float targetBankAngle = transform.eulerAngles.z - inputHandler.bankValue * rotationAmount;
        targetBankAngle = targetBankAngle < 0 ? 360 + targetBankAngle : targetBankAngle;

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetBankAngle);

        if (inputHandler.bankValue == 0f) targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, originalRotationZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothFactor);

        if (Mathf.Abs(transform.eulerAngles.z - originalRotationZ) > 45f) originalRotationZ = Mathf.Round(transform.eulerAngles.z / 90f) * 90f;
    }

    private IEnumerator DelayedBankingToggle(float value = 1f)
    {
        bankIsActive = false;
        yield return new WaitForSeconds(value);
        bankIsActive = true;
    }

    void HandlerMovement()
    {
        float speedMultiplier = sprint ? sprintMultiplier : 1f;
        float speed = Speed * speedMultiplier;

        Vector2 moveInput = inputHandler.MoveInput;
        Vector3 inputDirection = new Vector3(moveInput.x, inputHandler.FlyValue, moveInput.y).normalized;

        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        Vector3 targetMovement = worldDirection * speed;

        currentMovement = Vector3.Lerp(currentMovement, targetMovement, movementSmoothFactor * Time.deltaTime);
        characterController.Move(currentMovement * Time.deltaTime);
    }

    void HandlerRotation()
    {
        float mouseYInput = invertYAxis ? -inputHandler.LookInput.y : inputHandler.LookInput.y;

        float mouseXRotation = inputHandler.LookInput.x * mouseSensitivity * viewSmoothFactor;
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
