using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] GameObject saveGameDialog = null;
    [SerializeField] GameObject noSaveGameDialog = null;

    #region Menu Buttons

    public void NewGameDialogsYes()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(_newGameLevel);
#else
        SceneManager.LoadScene(1); //2
#endif
    }

    public void LoadGameDialogsYes()
    {
        if (PlayerPrefs.HasKey("SaveLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SaveLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            saveGameDialog.SetActive(false);
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

    #endregion

}
