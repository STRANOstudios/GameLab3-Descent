using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] bool canUse = true;
    [SerializeField] MenuController menuController;

    [Header("Audio Settings")]
    [SerializeField] Slider Master = null;
    [SerializeField] Slider Music = null;
    [SerializeField] Slider Fsx = null;
    [SerializeField] Slider Dialogue = null;

    [Header("Graphics Settings")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] TMP_Dropdown ResolutionDropdown;
    [SerializeField] Toggle fullscreen = null;
    [SerializeField] Toggle raytracing = null;
    [SerializeField] Toggle pathtracing = null;


    [Header("Gameplay Settings")]
    [SerializeField] TMP_Text controllerSenTextValue = null;
    [SerializeField] Slider controllerSenSlider = null;
    [SerializeField] Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            PrintAllPlayerPrefsKeys();

            //if (PlayerPrefs.HasKey("masterVolume"))
            //{
            //    float localVolume = PlayerPrefs.GetFloat("masterVolume");
            //}
        }
    }

    void PrintAllPlayerPrefsKeys()
    {
        Debug.Log("Stampa tutte le chiavi PlayerPrefs:");

        // Usa reflection per ottenere le chiavi private
        System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
        typeof(PlayerPrefs).GetField("m_PlayerPrefs", flags).GetValue(null);

        // Accedi alle chiavi private
        string[] keys = (string[])typeof(PlayerPrefs).GetField("m_StringKeys", flags).GetValue(null);

        // Stampa tutte le chiavi
        foreach (string key in keys)
        {
            Debug.Log("Chiave: " + key);
        }
    }
}
