using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] GameObject noSaveGameDialog = null;

    [Header("Gameplay Settings")]
    [SerializeField] TMP_Text controllerSenTextValue = null;
    [SerializeField] Slider controllerSenSlider = null;
    [SerializeField] int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle settings")]
    [SerializeField] Toggle invertYToggle = null;

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

    public void SetControllerSen(float value)
    {
        mainControllerSen=Mathf.RoundToInt(value);
        controllerSenTextValue.text = value.ToString("0");
    }

    public void GameplayApply()
    {
        if(invertYToggle.isOn) PlayerPrefs.SetInt("masterInvertY", 1);
        else PlayerPrefs.SetInt("masterInvertY", 0);

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    }
}
