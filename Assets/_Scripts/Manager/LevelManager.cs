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
            Debug.Log("pause");

            StartCoroutine(Delay(pauseIsActive));
            isGamePaused = !isGamePaused;
            Time.timeScale = isGamePaused ? 0 : 1;
        }
    }

    public void OpenReactor()
    {
        isReactorOpened = true;
    }

    private IEnumerator Delay(bool var, float value = 1f)
    {
        var = false;
        yield return new WaitForSeconds(value);
        var = true;
    }
}
