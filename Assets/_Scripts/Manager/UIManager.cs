using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static LevelManager;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    //top
    [Header("Top Elements")]
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text mission;

    //key
    [Header("Key Elements")]
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject greenLight;
    [SerializeField] GameObject blueLight;
    [Space]
    [SerializeField] Material materialLight;

    //bottom
    [Header("Energy Elements")]
    [SerializeField] TMP_Text energy;
    [SerializeField] Slider energySliderLeft;
    [SerializeField] Slider energySliderRight;

    [Header("Shield Elements")]
    [SerializeField] TMP_Text shield;

    //monitor left
    [Header("Monitors Elements")]
    [Header("left")]
    [SerializeField] Image gunBannerLeft;
    [SerializeField] TMP_Text gunNameLeft;
    [SerializeField] TMP_Text gunMagazineLeft;

    //monitor right
    [Header("right")]
    [SerializeField] Image gunBannerRigth;
    [SerializeField] TMP_Text gunNameRigth;
    [SerializeField] TMP_Text gunMagazineRigth;

    [Header("Center Monitor Elements")]
    [SerializeField] GameObject monitor;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject miniMap;

    private void OnEnable()
    {
        Gun.shoot += Energy;
        HealthManager.healt += Shield;

        LevelManager.pause += Pause;
        PlayerController.map += MiniMap;
    }
    private void OnDisable()
    {
        Gun.shoot -= Energy;
        HealthManager.healt -= Shield;

        LevelManager.pause -= Pause;
        PlayerController.map -= MiniMap;
    }

    private void Energy(int value)
    {
        string formattedValue = value.ToString("D3");
        energy.text = formattedValue;
        energySliderLeft.value = value;
        energySliderRight.value = value;
    }

    private void Shield(int value)
    {
        string formattedValue = value.ToString("D3");
        shield.text = formattedValue;
    }

    private void Pause(bool value)
    {
        pauseMenu.SetActive(value);

        MonitorAnimation(value);
    }

    private void MiniMap(bool value)
    {
        miniMap.SetActive(value);

        MonitorAnimation(value);
    }

    private void MonitorAnimation(bool value)
    {
        monitor.SetActive(value);
    }
}
