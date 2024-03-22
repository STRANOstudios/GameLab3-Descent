using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [Header("Graphics Settings")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullScreenToggle = null;
    [SerializeField] TMP_Dropdown qualityDropdown;

    private Volume volume = null;

    private Resolution[] resolutions;

    private int _qualityLevel;
    private bool _isFullScreen;
    private int _brightnessLevel;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        volume = GameManager.Instance.VolumeBrightness;

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
    }

    void OnDisable()
    {
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
    }

    void SetBrightness(float value)
    {
        _brightnessLevel = (int)value;
        PlayerPrefs.SetInt("Brightness", _brightnessLevel);

        if (!volume.profile.TryGet(out colorAdjustments)) return;

        Color newColor = new(_brightnessLevel / 255f, _brightnessLevel / 255f, _brightnessLevel / 255f, 1);
        colorAdjustments.colorFilter.Override(newColor);
    }

    void SetResolution(int value)
    {
        Resolution resolution = resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetFloat("Resolution_width", resolution.width);
        PlayerPrefs.SetFloat("Resolution_height", resolution.height);
    }

    void SetFullScreen(bool value)
    {
        _isFullScreen = value;
        Screen.fullScreen = _isFullScreen;
        PlayerPrefs.SetInt("FullScreen", (_isFullScreen ? 1 : 0));
    }

    void SetQuality(int value)
    {
        _qualityLevel = value;
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("Quality", _qualityLevel);
    }

    void SetGraphics()
    {
        brightnessSlider.value = GetSavedInt("Brightness");
        bool _isFullScreen = GetSavedInt("FullScreen") == 1;
        Screen.SetResolution((int)GetSavedFloat("Resolution_width"), (int)GetSavedFloat("Resolution_height"), _isFullScreen);
        fullScreenToggle.isOn = _isFullScreen;
        qualityDropdown.SetValueWithoutNotify((int)GetSavedInt("Quality"));
    }

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