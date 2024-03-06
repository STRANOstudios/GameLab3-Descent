using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    public string levelToLoad;
    [SerializeField] GameObject noSaveGameDialog = null;

    public void NewGameDialogsYes()
    {
        SceneManager.LoadScene(_newGameLevel);
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
        Application.Quit();
    }
}
