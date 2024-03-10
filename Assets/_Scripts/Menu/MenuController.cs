using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

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

    [SerializeField] Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] float defaultBrightness = 1.0f;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown ResolutionDropdown;
    private Resolution[] resolutions;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private void Start()
    {
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int value)
    {
        Resolution resolution = resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

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
        mainControllerSen = Mathf.RoundToInt(value);
        controllerSenTextValue.text = value.ToString("0");
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn) PlayerPrefs.SetInt("masterInvertY", 1);
        else PlayerPrefs.SetInt("masterInvertY", 0);

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    }

    public void SetBrightness(float value)
    {
        _brightnessLevel = value;

    }

    public void SetFullScreen(bool value)
    {
        _isFullScreen = value;
    }

    public void SetQuality(int value)
    {
        _qualityLevel = value;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("MasterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("MasterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
    }

    public void SetAudio(Slider value)
    {
        PlayerPrefs.SetInt($"{value.name}", (int)value.value);
    }

    public void SetDynamicRange(int value)
    {
        PlayerPrefs.SetInt("dynamicRange", value);
    }
}
