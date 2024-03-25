using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] PlayerInputHadler inputHandler;
    [SerializeField] Volume volumeBrightness;
    [SerializeField] Volume volumeHD;
    [SerializeField] Volume volumeRayTracing;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioClip music;

    private bool invertYAxis;
    private float mouseSensitivity;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameplaySettings.settings += SetGameplay;
    }

    private void OnDisable()
    {
        GameplaySettings.settings += SetGameplay;
    }

    public void ReturnToMenu()
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public void volume(bool value)
    {
        if (value)
        {
            volumeHD.enabled = false;
            volumeRayTracing.enabled = true;
        }
        else
        {
            volumeHD.enabled = true;
            volumeRayTracing.enabled = false;
        }
    }
}
