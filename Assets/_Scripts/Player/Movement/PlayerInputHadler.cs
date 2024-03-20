using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHadler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Rederences")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Cation Name Refernces")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string fly = "Fly";
    [SerializeField] private string bankIsActive = "Bank <activate>";
    [SerializeField] private string bank = "Bank <action>";
    [SerializeField] private string fire1 = "Primary Fire";
    [SerializeField] private string fire2 = "Secondary Fire";
    [SerializeField] private string bomb = "Bomb";
    [SerializeField] private string flare = "Flare";
    [SerializeField] private string map = "Map";
    [SerializeField] private string rearView = "Rear View";
    [SerializeField] private string list1 = "Primary List";
    [SerializeField] private string list2 = "Secondary List";
    [SerializeField] private string pause = "Pause";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flyAction;

    private InputAction bankIsActiveAction;
    private InputAction bankAction;

    private InputAction fire1Action;
    private InputAction fire2Action;

    private InputAction list1Action;
    private InputAction list2Action;

    private InputAction bombAction;
    private InputAction flareAction;

    private InputAction mapAction;

    private InputAction rearViewAction;

    private InputAction pauseAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float FlyValue { get; private set; }
    public bool bankIsActiveTrigger { get; private set; }
    public float bankValue { get; private set; }
    public bool fire1Trigger { get; private set; }
    public bool fire2Trigger { get; private set; }
    public float list1Value { get; private set; }
    public float list2Value { get; private set; }
    public bool bombTrigger { get; private set; }
    public bool flareTrigger { get; private set; }
    public bool mapTrigger { get; private set; }
    public bool rearViewTrigger { get; private set; }
    public bool pauseTrigger { get; private set; }

    public static PlayerInputHadler Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        #endregion

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        flyAction = playerControls.FindActionMap(actionMapName).FindAction(fly);

        bankIsActiveAction = playerControls.FindActionMap(actionMapName).FindAction(bankIsActive);
        bankAction = playerControls.FindActionMap(actionMapName).FindAction(bank);

        fire1Action = playerControls.FindActionMap(actionMapName).FindAction(fire1);
        fire2Action = playerControls.FindActionMap(actionMapName).FindAction(fire2);

        list1Action = playerControls.FindActionMap(actionMapName).FindAction(list1);
        list2Action = playerControls.FindActionMap(actionMapName).FindAction(list2);

        bombAction = playerControls.FindActionMap(actionMapName).FindAction(bomb);
        flareAction = playerControls.FindActionMap(actionMapName).FindAction(flare);

        mapAction = playerControls.FindActionMap(actionMapName).FindAction(map);
        rearViewAction = playerControls.FindActionMap(actionMapName).FindAction(rearView);

        pauseAction = playerControls.FindActionMap(actionMapName).FindAction(pause);

        RegisterInputActions();

        //PrintDevice();
    }

    void PrintDevice()
    {
        foreach(var device in InputSystem.devices)
        {
            if (device.enabled) Debug.Log($"Active Device: {device.name}");
        }
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        flyAction.performed += context => FlyValue = context.ReadValue<float>();
        flyAction.canceled += context => FlyValue = 0f;

        bankIsActiveAction.performed += context => bankIsActiveTrigger = true;
        bankIsActiveAction.canceled += context => bankIsActiveTrigger = false;

        bankAction.performed += context => bankValue = context.ReadValue<float>();
        bankAction.canceled += context => bankValue = 0f;

        list1Action.performed += context => list1Value = context.ReadValue<float>();
        list1Action.canceled += context => list1Value = 0f;

        list2Action.performed += context => list2Value = context.ReadValue<float>();
        list2Action.canceled += context => list2Value = 0f;

        fire1Action.performed += context => fire1Trigger = true;
        fire1Action.canceled += context => fire1Trigger = false;

        fire2Action.performed += context => fire2Trigger = true;
        fire2Action.canceled += context => fire2Trigger = false;

        bombAction.performed += context => bombTrigger = true;
        bombAction.canceled += context => bombTrigger = false;

        flareAction.performed += context => flareTrigger = true;
        flareAction.canceled += context => flareTrigger = false;

        mapAction.performed += context => mapTrigger = true;
        mapAction.canceled += context => mapTrigger = false;

        rearViewAction.performed += context => rearViewTrigger = true;
        rearViewAction.canceled += context => rearViewTrigger = false;

        pauseAction.performed += context => pauseTrigger = true;
        pauseAction.canceled += context => pauseTrigger = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        flyAction.Enable();
        bankIsActiveAction.Enable();
        bankAction.Enable();
        fire1Action.Enable();
        fire2Action.Enable();
        list1Action.Enable();
        list2Action.Enable();
        bombAction.Enable();
        flareAction.Enable();
        mapAction.Enable();
        rearViewAction.Enable();
        pauseAction.Enable();

        //InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        flyAction.Disable();
        bankIsActiveAction.Disable();
        bankAction.Disable();
        fire1Action.Disable();
        fire2Action.Disable();
        list1Action.Disable();
        list2Action.Disable();
        bombAction.Disable();
        flareAction.Disable();
        mapAction.Disable();
        rearViewAction.Disable();
        pauseAction.Disable();

        //InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                Debug.Log($"Device Disconnected: {device.name}");
                //hadle disconnection
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log($"Device Reconnected: {device.name}");
                //hadle reconnected
                break;
        }
    }
}
