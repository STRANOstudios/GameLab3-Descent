using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] GameObject reactor;
    [SerializeField] List<GameObject> Hostages;
    [Space]
    [SerializeField] float timeToEscape = 30f;

    private PlayerInputHadler inputHandler;
    private bool isGamePaused = false;
    private bool isReactorOpened = false;
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

        if (isReactorOpened)
        {
            if (!reactor.activeSelf)
            {
                Wait(timeToEscape);
            }
        }
    }

    private void OnEnable()
    {
        UIManager.resume += Resume;
    }

    private void OnDisable()
    {
        UIManager.resume -= Resume;
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

    private IEnumerator Delay(float value = 1f)
    {
        pauseIsActive = false;
        yield return new WaitForSecondsRealtime(value);
        pauseIsActive = true;
    }

    private IEnumerator Wait(float value = 1f)
    {
        yield return new WaitForSecondsRealtime(value);
        if (!successfulEscape)
        {
            Debug.Log("you died");
            //return to main menu
        }
    }
}
