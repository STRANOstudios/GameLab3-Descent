using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] PlayerInputHadler inputHandler;

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

        inputHandler = PlayerInputHadler.Instance;
    }

    private void OnEnable()
    {
        HealthManager.dead += ReturnToMenu;
    }

    private void OnDisable()
    {
        HealthManager.dead -= ReturnToMenu;
    }

    public void ReturnToMenu()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene(0); //1
#endif
    }

    public PlayerInputHadler GetInputHandler => inputHandler;
}
