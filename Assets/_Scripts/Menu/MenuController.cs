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
    [SerializeField] GameObject saveGameDialog = null;
    [SerializeField] GameObject noSaveGameDialog = null;

    [Header("Gameplay Settings")]
    [SerializeField] TMP_Text controllerSenTextValue = null;
    [SerializeField] Slider controllerSenSlider = null;
    [SerializeField] int defaultSen = 4;
    public int mainControllerSen = 4;

    [SerializeField] Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullScreenToggle = null;
    [SerializeField] TMP_Dropdown qualityDropdown;

    private Resolution[] resolutions;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    private void Start()
    {
        SetGraphics();

        #region resolution dropdown values settings
        resolutions = Screen.resolutions;

        List<string> options = new();

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

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endregion
    }

    void OnEnable()
    {
        #region Gameplay Settings addlisteners
        #endregion

        #region Graphics Settings addlisteners
        brightnessSlider.onValueChanged.AddListener(delegate
        {
            SetBrightness(brightnessSlider.value);
        });
        resolutionDropdown.onValueChanged.AddListener(delegate
        {
            SetResolution(resolutionDropdown.value);
        });
        fullScreenToggle.onValueChanged.AddListener(delegate
        {
            SetFullScreen(fullScreenToggle.isOn);
        });
        qualityDropdown.onValueChanged.AddListener(delegate
        {
            SetQuality(qualityDropdown.value);
        });
        #endregion
    }

    void OnDisable()
    {
        #region Gameplay Settings removelisteners
        #endregion

        #region Graphics Settings removelisteners
        brightnessSlider.onValueChanged.RemoveListener(delegate
        {
            SetBrightness(brightnessSlider.value);
        });
        resolutionDropdown.onValueChanged.RemoveListener(delegate
        {
            SetResolution(resolutionDropdown.value);
        });
        fullScreenToggle.onValueChanged.RemoveListener(delegate
        {
            SetFullScreen(fullScreenToggle.isOn);
        });
        qualityDropdown.onValueChanged.RemoveListener(delegate
        {
            SetQuality(qualityDropdown.value);
        });
        #endregion
    }

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

    #region Gameplay

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

        //to be implemented
    }

    #endregion

    #region Graphics

    public void SetBrightness(float value)
    {
        _brightnessLevel = value;
        PlayerPrefs.SetFloat("Brightness", _brightnessLevel);

        //to be implemented
        //volume exposure
    }

    public void SetResolution(int value)
    {
        Resolution resolution = resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetFloat("Resolution_width", resolution.width);
        PlayerPrefs.SetFloat("Resolution_height", resolution.height);
    }

    public void SetFullScreen(bool value)
    {
        _isFullScreen = value;
        Screen.fullScreen = _isFullScreen;
        PlayerPrefs.SetInt("FullScreen", (_isFullScreen ? 1 : 0));
    }

    public void SetQuality(int value)
    {
        _qualityLevel = value;
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("Quality", _qualityLevel);
    }

    public void SetGraphics()
    {
        brightnessSlider.value = GetSavedFloat("Brightness");
        bool _isFullScreen = GetSavedInt("FullScreen") == 1;
        Screen.SetResolution((int)GetSavedFloat("Resolution_width"), (int)GetSavedFloat("Resolution_height"), _isFullScreen);
        fullScreenToggle.isOn = _isFullScreen;
        qualityDropdown.SetValueWithoutNotify((int)GetSavedInt("Quality"));
    }

    #endregion

    float GetSavedFloat(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetFloat(key);
        return 0f;
    }

    float GetSavedInt(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key);
        return 0;
    }
}
