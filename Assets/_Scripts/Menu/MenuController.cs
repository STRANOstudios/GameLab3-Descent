using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] GameObject noSaveGameDialog = null;

    public void NewGameDialogsYes()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(_newGameLevel);
#else
        SceneManager.LoadScene(2);
#endif
    }

    public void LoadGameDialogsYes()
    {
        if (PlayerPrefs.HasKey("SaveLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSaveGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
