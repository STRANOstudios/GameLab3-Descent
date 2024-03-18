using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] GameObject reactor;
    [SerializeField] List<GameObject> Hostages;

    private PlayerInputHadler inputHandler;
    private bool isGamePaused = false;

    private bool isReactorOpened = false;

    private bool pauseIsActive = true;

    public delegate void Pause(bool value);
    public static event Pause pause = null;

    private void Start()
    {
        inputHandler = PlayerInputHadler.Instance;
    }

    private void Update()
    {
        PauseState();

        //if (isReactorOpened)
        //{
        //    if (!reactor.activeSelf)
        //    {

        //        //timer activated n.sec to escape from base

        //        //if you can escape ...
        //            if (Hostages.Count == 0)
        //            {
        //                //good ending
        //                Debug.Log("good ending");
        //            }
        //            else
        //            {
        //                //bad ending
        //                Debug.Log("bad ending");
        //            }
        //            //next level

        //        //else you died
        //            //return to main menu

        //    }
        //}
    }

    private void OnEnable()
    {
        //to be implemented delegate from reactor door
    }

    private void OnDisable()
    {
        //to be implemented delegate from reactor door
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

    public void OpenReactor()
    {
        isReactorOpened = true;
    }

    private IEnumerator Delay(float value = 1f)
    {
        pauseIsActive = false;
        yield return new WaitForSecondsRealtime(value);
        pauseIsActive = true;
    }
}
