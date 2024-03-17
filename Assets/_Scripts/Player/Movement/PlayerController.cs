using System.Collections;
using UnityEngine;

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
    [SerializeField] Transform mainCamera;
    [SerializeField] GameObject UI_FrontView;
    [SerializeField] GameObject UI_RearView;
    [Space]
    [SerializeField] float oscillationAmount = 1f;
    [SerializeField] float oscillationSpeed = 1f;
    [SerializeField] float floatingOscillationAmount = 0.5f;
    [SerializeField] float floatingOscillationSpeed = 1f;

    private CharacterController characterController;
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
        inputHandler = PlayerInputHadler.Instance;
    }

    private void Update()
    {
        HandlerMovement();
        HandlerRotation();
        HandlerBanking();

        RearView();

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
        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized; 

        Vector3 cameraForward = mainCamera.forward;
        Vector3 cameraRight = mainCamera.right;
        Vector3 horizontalMovement = (cameraForward * inputDirection.z + cameraRight * inputDirection.x) * speed;

        Vector3 flyMovement = Vector3.up * inputHandler.FlyValue * speed;

        Vector3 targetMovement = horizontalMovement + flyMovement;

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
        mainCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        float horizontalRotation = Mathf.Abs(inputHandler.LookInput.x);
        float targetOscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmount * horizontalRotation;

        currentOscillation = Mathf.SmoothDamp(currentOscillation, targetOscillation, ref currentOscillationVelocity, acceleration);

        mainCamera.Rotate(0, 0, currentOscillation);
    }

    void RearView()
    {
        if (inputHandler.rearViewTrigger)
        {
            mainCamera.rotation = Quaternion.Euler(-mainCamera.rotation.eulerAngles.x, (mainCamera.rotation.eulerAngles.y + 180f + 360f) % 360f, mainCamera.rotation.eulerAngles.z);

            //UI
            UI_FrontView.SetActive(false);
            UI_RearView.SetActive(true);
        }
        else
        {
            //UI
            UI_FrontView.SetActive(true);
            UI_RearView.SetActive(false);
        }
    }

    //vfx
    void ApplyFloatingOscillation()
    {
        float floatingOscillation = Mathf.Sin(Time.time * floatingOscillationSpeed) * floatingOscillationAmount;

        mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, floatingOscillation, mainCamera.localPosition.z);
    }
}
