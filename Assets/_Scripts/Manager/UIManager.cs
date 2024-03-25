using DevionGames;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.ReloadAttribute;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    //top
    [Header("Top Elements")]
    [SerializeField] CurvyText score;
    [SerializeField] CurvyText mission;

    //key
    [Header("Key Elements")]
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject yellowLight;
    [SerializeField] GameObject blueLight;
    [Space]
    [SerializeField] Material materialRed;
    [SerializeField] Material materialYellow;
    [SerializeField] Material materialBlue;

    //bottom
    [Header("Energy Elements")]
    [SerializeField] TMP_Text energy;
    [SerializeField] Slider energySliderLeft;
    [SerializeField] Slider energySliderRight;

    [Header("Shield Elements")]
    [SerializeField] Image shieldBar;
    [SerializeField] List<Sprite> shields = new();
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

    private int scoreValue = 0;
    private int packages = 0;

    private string mainMission = "Destroy the reactor";
    private string optionable = $"Retrieve packages 0/5";

    public delegate void Resum();
    public static event Resum resume = null;

    private void OnEnable()
    {
        Gun.shoot += Energy;
        HealthManager.healt += Shield;

        LevelManager.pause += Pause;
        PlayerController.map += MiniMap;
        ShootingManager.Gun += GunsMonitors;
        HealthManager.healt += Shield;

        Score.OnObjectDeactivated += ScoreSet;
        PlayerKeyHolder.OnKeyPickUp += AddKey;

        AmazonVan.delivered += Box;

        CoreLogic.death -= Core;
    }
    private void OnDisable()
    {
        Gun.shoot -= Energy;
        HealthManager.healt -= Shield;

        LevelManager.pause -= Pause;
        PlayerController.map -= MiniMap;
        ShootingManager.Gun -= GunsMonitors;
        HealthManager.healt -= Shield;

        Score.OnObjectDeactivated -= ScoreSet;
        PlayerKeyHolder.OnKeyPickUp -= AddKey;

        AmazonVan.delivered -= Box;

        CoreLogic.death -= Core;
    }

    private void Box()
    {
        packages++;
        optionable = $"Retrieve packages {packages}/5";

        UpdateMessage();
    }

    private void Core()
    {
        mainMission = "Escape from base";

        UpdateMessage();
    }

    private void UpdateMessage()
    {
        mission.text = "Main mission: " + mainMission + "\n" + "Optional: " + optionable;
    }

    private void ScoreSet(int value)
    {
        scoreValue += value;
        if (score != null) score.text = scoreValue.ToString("D5") + " ";
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

        float percentage = (value / 200f) * 100f;
        int index = Mathf.RoundToInt(percentage / (100f / (shields.Count - 1)));
        index = Mathf.Clamp(index, 0, shields.Count - 1);
        shieldBar.sprite = shields[index];
    }

    void AddKey(int value)
    {
        switch (value)
        {
            case 1:
                redLight.GetComponent<Image>().material = materialRed;
                break;
            case 2:
                yellowLight.GetComponent<Image>().material = materialYellow;
                break;
            case 3:
                blueLight.GetComponent<Image>().material = materialBlue;
                break;
            default:
                break;
        }
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

    private void GunsMonitors(bool isPrimary, Sprite sprite, string name, int bulletMagazine)
    {
        Image image = isPrimary ? gunBannerLeft : gunBannerRigth;
        image.gameObject.SetActive(true);
        image.sprite = sprite;
        image.preserveAspect = true;

        TMP_Text text = isPrimary ? gunNameLeft : gunNameRigth;
        text.gameObject.SetActive(true);
        text.text = name;

        TMP_Text magazine = isPrimary ? gunMagazineLeft : gunMagazineRigth;
        magazine.gameObject.SetActive(true);
        magazine.text = bulletMagazine.ToString("D3");

        if (name == "Laser") gunMagazineLeft.gameObject.SetActive(false);
    }

    public void Resume()
    {
        resume?.Invoke();
    }
}
