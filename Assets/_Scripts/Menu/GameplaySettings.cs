using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HealthManager;
using static UnityEngine.Rendering.DebugUI;

public class GameplaySettings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] Slider controllerSenSlider = null;
    [SerializeField] Toggle invertYToggle = null;

    public delegate void GameplaySet(float controllerSen, bool invertY);
    public static event GameplaySet settings = null;

    void Start()
    {
        SetGameplay();
    }

    void OnEnable()
    {
        controllerSenSlider.onValueChanged.AddListener(delegate
        {
            SetControllerSen(controllerSenSlider.value);
        });
        invertYToggle.onValueChanged.AddListener(delegate
        {
            SetY(invertYToggle.isOn);
        });
    }

    void OnDisable()
    {
        controllerSenSlider.onValueChanged.RemoveListener(delegate
        {
            SetControllerSen(controllerSenSlider.value);
        });
        invertYToggle.onValueChanged.AddListener(delegate
        {
            SetY(invertYToggle.isOn);
        });
    }

    void SetControllerSen(float value)
    {
        settings?.Invoke(value, invertYToggle.isOn);
        PlayerPrefs.SetFloat("ControllerSen", value);
    }

    void SetY(bool value)
    {
        settings?.Invoke(controllerSenSlider.value, value);
        PlayerPrefs.SetInt("InvertY", (value ? 1 : 0));
    }

    void SetGameplay()
    {
        controllerSenSlider.value = GetSavedFloat("ControllerSen");
        invertYToggle.isOn = GetSavedInt("InvertY") == 1;

        settings?.Invoke(controllerSenSlider.value, invertYToggle.isOn);
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
