using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] PlayerInputHadler inputHandler;
    [SerializeField] Volume volumeBrightness;
    [SerializeField] PlayerController playerController;

    private bool invertYAxis;
    private float mouseSensitivity;

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

        //inputHandler = PlayerInputHadler.Instance;
    }

    private void OnEnable()
    {
        HealthManager.dead += ReturnToMenu;
        GameplaySettings.settings += SetGameplay;
    }

    private void OnDisable()
    {
        HealthManager.dead -= ReturnToMenu;
        GameplaySettings.settings += SetGameplay;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene(0); //1
#endif
    }

    public void SetGameplay(float controllerSen, bool invertY)
    {
        invertYAxis = invertY;
        mouseSensitivity = controllerSen;
    }

    public PlayerInputHadler InputHandler => inputHandler;
    public Volume VolumeBrightness => volumeBrightness;
    public PlayerController PlayerController => playerController;

    public bool InvertY => invertYAxis;
    public float MouseSensitivity => mouseSensitivity;
}
