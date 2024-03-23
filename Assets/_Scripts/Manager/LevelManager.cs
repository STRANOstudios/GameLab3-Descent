using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] GameObject reactor;
    [SerializeField] List<GameObject> Hostages;
    [Space]
    [SerializeField] float timeToEscape = 50f;

    [Header("UI")]
    [SerializeField] TMP_Text countDown;

    private PlayerInputHadler inputHandler;
    private bool isGamePaused = false;
    public bool isReactorOpened = false;
    private bool pauseIsActive = true;
    private bool successfulEscape = false;

    public delegate void Pause(bool value);
    public static event Pause pause = null;

    private void Start()
    {
        inputHandler = PlayerInputHadler.Instance;
    }

    private void Update()
    {
        PauseState();
    }

    private void OnEnable()
    {
        UIManager.resume += Resume;
        CoreLogic.death += CountDown;
    }

    private void OnDisable()
    {
        UIManager.resume -= Resume;
        CoreLogic.death -= CountDown;
    }

    private void PauseState()
    {
        if (inputHandler.pauseTrigger && pauseIsActive)
        {
            StartCoroutine(Delay(0.3f));
            isGamePaused = !isGamePaused;
            pause?.Invoke(isGamePaused);
            Time.timeScale = isGamePaused ? 0 : 1;
        }
    }

    private void Resume()
    {
        isGamePaused = false;
        pause?.Invoke(isGamePaused);
        Time.timeScale = 1;
    }

    public void OpenReactor()
    {
        isReactorOpened = true;
    }

    void Escape()
    {
        successfulEscape = true;

        if (FreeHostage())
        {
            //good ending
            Debug.Log("good ending");
        }
        else
        {
            //bad ending
            Debug.Log("bad ending");
        }

        //return to main menu
    }

    bool FreeHostage()
    {
        foreach (var hostage in Hostages)
        {
            if (hostage.activeSelf) return false;
        }
        return true;
    }

    void CountDown()
    {
        countDown.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator Delay(float value = 1f)
    {
        pauseIsActive = false;
        yield return new WaitForSecondsRealtime(value);
        pauseIsActive = true;
    }

    IEnumerator StartCountdown()
    {
        
        float timeRemaining = timeToEscape;

        while (timeRemaining > 0f)
        {
            countDown.text = timeRemaining.ToString("00");

            yield return new WaitForSeconds(1f);

            timeRemaining -= 1f;
        }
        countDown.text = "00";
        if (!successfulEscape)
        {
            Debug.Log("you died");
            //return to main menu
        }
    }
}
