using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Language language;

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
    }

    private void OnEnable()
    {
        HealthManager.Death += ReturnToMenu;
    }

    private void OnDisable()
    {
        HealthManager.Death -= ReturnToMenu;
    }

    public void ReturnToMenu()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene(0); //1
#endif
    }
}
