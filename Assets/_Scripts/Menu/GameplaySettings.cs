using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] TMP_Text controllerSenTextValue = null;
    [SerializeField] Slider controllerSenSlider = null;
    [SerializeField] int defaultSen = 4;
    public int mainControllerSen = 4;

    [SerializeField] Toggle invertYToggle = null;

    PlayerController playerController = null;

    void Start()
    {
        playerController = GameManager.Instance.PlayerController;

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
        mainControllerSen = Mathf.RoundToInt(value);
        controllerSenTextValue.text = value.ToString("D1");
    }

    void SetY(bool value)
    {
        playerController.InvertY = value;
        PlayerPrefs.SetInt("FullScreen", (value ? 1 : 0));
    }

    void SetGameplay()
    {

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
