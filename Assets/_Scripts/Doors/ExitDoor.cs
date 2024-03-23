using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] float timer;

    Animator anim;
    private void OnEnable()
    {
        CoreLogic.death += Timer;
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        CoreLogic.death -= Timer;
    }
    private void Timer()
    {
        anim.SetBool("OpenDoor", true);
        StartCoroutine(EndTimer());
        
    }

    IEnumerator EndTimer()
    {
        yield return new WaitForSeconds(timer);
    }
}
